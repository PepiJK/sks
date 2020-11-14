using System;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using KochWermann.SKS.Package.ServiceAgents;
using KochWermann.SKS.Package.ServiceAgents.Exceptions;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class ServiceAgentsTests
    {
        private IGeoEncodingAgent _encoder;
        private readonly string _address = "Maschlgasse 90, 1220 Wien, Österreich";
        private readonly string _baseUrl = "https://someurl.com/search?email=if18b182@technikum-wien.at&format=geojson";

        [SetUp]
        public void Setup ()
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri == new Uri($"{_baseUrl}&q=")), ItExpr.IsAny<CancellationToken>())
                .Throws<InvalidCastException>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri == new Uri($"{_baseUrl}&q={_address}")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'features': [{'geometry': {'coordinates': [16.4813662, 48.2366093]}}]}"),
                });
            
            
            var client = new HttpClient(mockHttpMessageHandler.Object){
                BaseAddress = new Uri(_baseUrl)
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            _encoder = new OpenStreetMapEncodingAgent(new LoggerFactory().CreateLogger<OpenStreetMapEncodingAgent>(), mockFactory.Object);
        }

        [Test]
        public void AddressEncoder_OK()
        {
            var coordinates = _encoder.AddressEncoder(_address);
            Assert.AreEqual(16.4813662, coordinates.Lat);
            Assert.AreEqual(48.2366093, coordinates.Lon);
        }

        [Test]
        public void AddressEncoder_NotOK()
        {
            var ex = Assert.Throws<ServiceAgentNoResultException>(() => _encoder.AddressEncoder(""));
            StringAssert.Contains("No results found!", ex.Message);
        }
    }


}