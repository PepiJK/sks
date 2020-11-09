using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.ServiceAgents.Interfaces
{
    public interface IGeoEncodingAgent
    {
        GeoCoordinate AddressEncoder(string address);
    }
}