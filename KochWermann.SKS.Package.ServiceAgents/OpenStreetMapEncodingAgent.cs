using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using System.Net.Http;
using KochWermann.SKS.Package.ServiceAgents.Exceptions;

namespace KochWermann.SKS.Package.ServiceAgents
{
    public class OpenStreetMapEncodingAgent : IGeoEncodingAgent
    {
        private readonly ILogger<OpenStreetMapEncodingAgent> _logger;
        private readonly IHttpClientFactory _clientFactory;

        // Agent needed for Non-Browser Nominatim-API Requests
        private static readonly string _agent = "if18b182@technikum-wien.at";

        public OpenStreetMapEncodingAgent(ILogger<OpenStreetMapEncodingAgent> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public GeoCoordinate AddressEncoder(string address)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"search?email={_agent}&format=geojson&q={address}");
                var client = _clientFactory.CreateClient("openstreetmap");
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);
                    var json = streamReader.ReadToEnd();
                    dynamic res = JsonConvert.DeserializeObject(json);

                    return new GeoCoordinate{Lat = (double)res.features[0].geometry.coordinates[0], Lon = (double)res.features[0].geometry.coordinates[1]};
                }
                else
                {
                    throw new ServiceAgentRequestException("Openstreetmap request error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new ServiceAgentNoResultException("No results found!", ex);
            }
        }
    }
}