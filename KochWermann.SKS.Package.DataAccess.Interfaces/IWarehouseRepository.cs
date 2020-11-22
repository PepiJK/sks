using KochWermann.SKS.Package.DataAccess.Entities;
using System.Collections.Generic;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        string Create(Hop hop);
        void Update(Hop hop);
        void Delete(string code);
        void ClearAllTables();

        Hop GetHopByCode(string code);
        Warehouse GetRootWarehouse();
        Warehouse GetWarehouseByCode(string code);
        IEnumerable<Truck> GetAllTrucks();
        IEnumerable<Warehouse> GetAllWarehouses();
        Hop GetHopByCoordinates(double longitude, double latitude);
    }
}