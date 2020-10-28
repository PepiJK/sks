using KochWermann.SKS.Package.DataAccess.Entities;
using System.Collections.Generic;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        int Create(Hop hop);
        void Update(Hop hop);
        void Delete(int id);

        Hop GetHopById(int id);
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