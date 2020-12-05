using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using KochWermann.SKS.Package.Services.DTOs;
using Newtonsoft.Json;
using NUnit.Framework;

namespace KochWermann.SKS.Package.IntegrationTests
{
    // to run use following command: dotnet test --filter FullyQualifiedName\~IntegrationTests -s KochWermann.SKS.Package.IntegrationTests/local.runsettings
    [TestFixture, Category("IntegrationTests")]
    public class IntegrationTests
    {
        private readonly HttpClient _client = new HttpClient();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string baseUrl = TestContext.Parameters["baseUrl"];

            // import light sample dataset again to clear database
            var dataset = JsonConvert.DeserializeObject(File.ReadAllText("./trucks-new2-light-transferwh.json"));

            var res = _client.PostAsJsonAsync($"{baseUrl}/warehouse", dataset).Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        [Test]
        public void Should_Submit_Track_Report_Parcel()
        {
            string baseUrl = TestContext.Parameters["baseUrl"];

            // Submit new parcel
            var newParcel = new Parcel {
                Sender = new Recipient {
                    Name = "Josef Koch",
                    Street = "Am Spitz 11",
                    City = "Wien",
                    PostalCode = "A-1210",
                    Country = "Österreich"
                },
                Recipient = new Recipient {
                    Name = "Joe Wermann",
                    Street = "Zschokkegasse 35-29",
                    City = "Wien",
                    PostalCode = "A-1220",
                    Country = "Österreich"
                },
                Weight = 69
            };

            var res = _client.PostAsJsonAsync($"{baseUrl}/parcel", newParcel).Result;
            var newParcelInfo = res.Content.ReadAsAsync<NewParcelInfo>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsNotEmpty(newParcelInfo.TrackingId);

            // Track parcel
            var trackingId = newParcelInfo.TrackingId;

            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}").Result;
            var trackingInformation = res.Content.ReadAsAsync<TrackingInformation>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsNotEmpty(trackingInformation.FutureHops);
            Assert.IsEmpty(trackingInformation.VisitedHops);
            Assert.AreEqual(TrackingInformation.StateEnum.PickupEnum, trackingInformation.State);

            // Report parcel arrival at first hop
            var firstHopCode = trackingInformation.FutureHops[0].Code;

            res = _client.PostAsync($"{baseUrl}/parcel/{trackingId}/reportHop/{firstHopCode}", null).Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

            // Track parcel again to check reported hop
            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}").Result;
            trackingInformation = res.Content.ReadAsAsync<TrackingInformation>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.AreEqual(firstHopCode, trackingInformation.VisitedHops[0].Code);
            Assert.IsFalse(trackingInformation.FutureHops.Any(hop => hop.Code == firstHopCode));
            Assert.AreEqual(TrackingInformation.StateEnum.InTruckDeliveryEnum, trackingInformation.State);

            // Report final delivery
            res = _client.PostAsync($"{baseUrl}/parcel/{trackingId}/reportDelivery", null).Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

            // Track parcel again to check if it was deliverd
            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}").Result;
            trackingInformation = res.Content.ReadAsAsync<TrackingInformation>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsNotEmpty(trackingInformation.VisitedHops);
            Assert.IsEmpty(trackingInformation.FutureHops);
            Assert.AreEqual(TrackingInformation.StateEnum.DeliveredEnum, trackingInformation.State);
        }

        [Test]
        public void Should_Get_Create_Remove_Webhooks()
        {
            string baseUrl = TestContext.Parameters["baseUrl"];

            // Create parcel and new webhook
            // Submit new parcel
            var newParcel = new Parcel {
                Sender = new Recipient {
                    Name = "Josef Koch",
                    Street = "Am Spitz 11",
                    City = "Wien",
                    PostalCode = "A-1210",
                    Country = "Österreich"
                },
                Recipient = new Recipient {
                    Name = "Joe Wermann",
                    Street = "Zschokkegasse 35-29",
                    City = "Wien",
                    PostalCode = "A-1220",
                    Country = "Österreich"
                },
                Weight = 69
            };

            var res = _client.PostAsJsonAsync($"{baseUrl}/parcel", newParcel).Result;
            var newParcelInfo = res.Content.ReadAsAsync<NewParcelInfo>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsNotEmpty(newParcelInfo.TrackingId);

            // Create new webhook
            var trackingId = newParcelInfo.TrackingId;

            res = _client.PostAsync($"{baseUrl}/parcel/{trackingId}/webhooks?url=https://google.com", null).Result;
            var webhook = res.Content.ReadAsAsync<WebhookResponse>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.AreEqual(trackingId, webhook.TrackingId);

            // Get webhooks
            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}/webhooks").Result;
            var webhooks = res.Content.ReadAsAsync<IEnumerable<WebhookResponse>>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.AreEqual(webhook.Id, webhooks.First().Id);

            // Remove webhook
            var id = webhooks.First().Id;
            res = _client.DeleteAsync($"{baseUrl}/parcel/webhooks/{id}").Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

            // Get webhooks to check if deleted
            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}/webhooks").Result;
            webhooks = res.Content.ReadAsAsync<IEnumerable<WebhookResponse>>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsEmpty(webhooks);
        }

        [Test]
        public void Should_Import_Get_Warehouses()
        {
            string baseUrl = TestContext.Parameters["baseUrl"];

            // Submit new parcel
            var newParcel = new Parcel {
                Sender = new Recipient {
                    Name = "Josef Koch",
                    Street = "Am Spitz 11",
                    City = "Wien",
                    PostalCode = "A-1210",
                    Country = "Österreich"
                },
                Recipient = new Recipient {
                    Name = "Joe Wermann",
                    Street = "Zschokkegasse 35-29",
                    City = "Wien",
                    PostalCode = "A-1220",
                    Country = "Österreich"
                },
                Weight = 69
            };

            var res = _client.PostAsJsonAsync($"{baseUrl}/parcel", newParcel).Result;
            var newParcelInfo = res.Content.ReadAsAsync<NewParcelInfo>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsNotEmpty(newParcelInfo.TrackingId);
            
            // import light sample dataset
            var dataset = JsonConvert.DeserializeObject(File.ReadAllText("./trucks-new2-light-transferwh.json"));

            res = _client.PostAsJsonAsync($"{baseUrl}/warehouse", dataset).Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

            // Check that no parcels exist
            var trackingId = newParcelInfo.TrackingId;

            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}").Result;

            Assert.AreEqual(HttpStatusCode.NotFound, res.StatusCode);

            // Get new warehouse hierarchy
            res = _client.GetAsync($"{baseUrl}/warehouse").Result;
            var warehouse = res.Content.ReadAsAsync<Warehouse>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsTrue(warehouse.Level == 0);
            Assert.IsNotEmpty(warehouse.NextHops);

            // Get specific warehouse
            var code = warehouse.NextHops.First().Hop.Code;

            res = _client.GetAsync($"{baseUrl}/warehouse/{code}").Result;
            var hop = res.Content.ReadAsAsync<Hop>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.AreEqual(code, hop.Code);
            Assert.AreEqual("Warehouse", hop.HopType);
        }

        [Test]
        public void Should_Except_Parcel_From_Logistics_Partner()
        {
            string baseUrl = TestContext.Parameters["baseUrl"];

            // Submit existing parcel
            var existingParcel = new Parcel {
                Sender = new Recipient {
                    Name = "Slomo Koch",
                    Street = "Narof 8-27",
                    City = "Izlake",
                    PostalCode = "S-1411",
                    Country = "Slowenien"
                },
                Recipient = new Recipient {
                    Name = "Joe Wermann",
                    Street = "Zschokkegasse 35-29",
                    City = "Wien",
                    PostalCode = "A-1220",
                    Country = "Österreich"
                },
                Weight = 69
            };

            var trackingId = "PYJRB4HZ6";

            var res = _client.PostAsJsonAsync($"{baseUrl}/parcel/{trackingId}", existingParcel).Result;
            var newParcelInfo = res.Content.ReadAsAsync<NewParcelInfo>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.AreEqual(trackingId, newParcelInfo.TrackingId);

            // Get parcel
            res = _client.GetAsync($"{baseUrl}/parcel/{trackingId}").Result;
            var trackingInformation = res.Content.ReadAsAsync<TrackingInformation>().Result;

            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsNotEmpty(trackingInformation.FutureHops);
            Assert.IsEmpty(trackingInformation.VisitedHops);
            Assert.AreEqual("TWXX02", trackingInformation.FutureHops.First().Code);
            Assert.AreEqual(TrackingInformation.StateEnum.PickupEnum, trackingInformation.State);
        }
    }
}