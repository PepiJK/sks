using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        int Create(Warehouse warehouse);
        void Update(Warehouse warehouse);
        void Delete(int id);

        Warehouse  GetWarehouseById(int id);
        Warehouse GetWarehouseByCode(string code);
        Warehouse GetRootWarehouse();
        Hop GetHopByCode(string code);
        TransferWarehouse GetTransferWarehouseByCode(string code);
    }
}