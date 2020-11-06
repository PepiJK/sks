using KochWermann.SKS.Package.DataAccess.Entities;
using System.Collections.Generic;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        string Create(Hop hop);
        void Update(Hop hop);
        void Delete(string code);

        Hop GetHopByCode(string code);
        Warehouse GetRootWarehouse();
        TransferWarehouse GetTransferWarehouseByCode(string code);
        Warehouse GetWarehouseByCode(string code);
        IEnumerable<Hop> GetAllHops();
        IEnumerable<Truck> GetAllTrucks();
        IEnumerable<Warehouse> GetAllWarehouses();
        IEnumerable<WarehouseNextHops> GetAllWarehouseNextHops();
    }
}