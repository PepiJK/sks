using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        /// <summary>
        /// Exports the hierachy of warehouse and truck objects.
        /// </summary>
        Warehouse ExportWarehouses();

        /// <summary>
        /// Imports a hierachy of warehouse and truck objects.
        /// </summary>
        void ImportWarehouses(Warehouse warehouse);

        /// <summary>
        /// Get a certain warehouse or truck by code.
        /// </summary>
        Warehouse GetWarehouse(string code);
    }
}