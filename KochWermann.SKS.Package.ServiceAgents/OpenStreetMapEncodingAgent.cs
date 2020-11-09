using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.ServiceAgents
{
    public class OpenStreetMapEncodingAgent : IGeoEncodingAgent
    {
        private readonly ILogger<OpenStreetMapEncodingAgent> _logger;

        // Agent needed for Non-Browser Nominatim-API Requests
        private static readonly string _agent = "if18b182@technikum-wien.at";

        public OpenStreetMapEncodingAgent(ILogger<OpenStreetMapEncodingAgent> logger)
        {
            _logger = logger;
        }

        public GeoCoordinate AddressEncoder(string address)
        {
            try
            {
                string url = $"https://nominatim.openstreetmap.org/search?&q={address}&email={_agent}&format=geojson";
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());
                var json = streamReader.ReadToEnd();
                dynamic deserialized = JsonConvert.DeserializeObject(json);
                
                return new GeoCoordinate((double)deserialized.features[0].geometry.coordinates[0],(double)deserialized.features[0].geometry.coordinates[1]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("No results found!", ex);
            }
        }
    }
}