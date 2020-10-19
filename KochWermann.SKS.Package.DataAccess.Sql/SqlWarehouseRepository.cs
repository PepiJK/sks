using Microsoft.EntityFrameworkCore;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlWarehouseRepository : IWarehouseRepository
    {
        private readonly DatabaseContext _context;

        public SqlWarehouseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Create(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
            return warehouse.Id;
        }

        public void Delete(int id)
        {
            var warehouse = _context.Warehouses
                .SingleOrDefault(e => e.Id == id);

            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();
        }

        public void Update(Warehouse warehouse)
        {
            var entity = _context.Warehouses.FirstOrDefault(item => item.Id == warehouse.Id);

            if (entity != null)
            {
                entity.HopType = warehouse.HopType;
                entity.Code = warehouse.Code;
                entity.Description = warehouse.Description;
                entity.ProcessingDelayMins = warehouse.ProcessingDelayMins;
                entity.NextHops = warehouse.NextHops;

                _context.Warehouses.Update(entity);
                _context.SaveChanges();
            }
        }

        
        public Warehouse GetWarehouseById(int id)
        {
            var warehouse = _context.Warehouses
                .SingleOrDefault(e => e.Id == id);

            return warehouse;
        }

        public Warehouse GetWarehouseByCode(string code)
        {
            var warehouse = _context.Warehouses
                .SingleOrDefault(e => e.Code == code);

            return warehouse;
        }

        public Warehouse GetRootWarehouse()
        {
            _context.Warehouses.Load();
            _context.WarehouseNextHops.Load();
            _context.Trucks.Load();
            _context.TransferWarehouses.Load();

            var rootWarehouse = _context.Warehouses
                .SingleOrDefault(e => e.IsRootWarehouse == true);
            return rootWarehouse;
        }

        public Hop GetHopByCode(string code)
        {
            var hop = _context.Hops
                .SingleOrDefault(e => e.Code == code);

            return hop;
        }

        public TransferWarehouse GetTransferWarehouseByCode(string code)
        {
            var warehouse = _context.TransferWarehouses
                .SingleOrDefault(e => e.Code == code);

            return warehouse;
        }
    }
}
