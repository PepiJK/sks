using System;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using KochWermann.SKS.Package.ServiceAgents;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class ServiceAgentsTests
    {
        private readonly OpenStreetMapEncodingAgent _encoder = new OpenStreetMapEncodingAgent(new LoggerFactory().CreateLogger<OpenStreetMapEncodingAgent>());

        [SetUp]
        public void Setup ()
        {
        }

        [Test]
        public void AddressEncoder_OK()
        {
            var coordinates = _encoder.AddressEncoder("Maschlgasse 90, 1220 Wien, Österreich");
            Assert.AreEqual(16.4813662, coordinates.Lat);
            Assert.AreEqual(48.2366093, coordinates.Lon);
        }

        [Test]
        public void AddressEncoder_NotOK()
        {
            var ex = Assert.Throws<Exception>(() => _encoder.AddressEncoder(""));
            StringAssert.Contains("No results found!", ex.Message);
        }
    }
}