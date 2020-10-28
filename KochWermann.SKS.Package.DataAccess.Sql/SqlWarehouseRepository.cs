using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlWarehouseRepository : IWarehouseRepository
    {
        private readonly IDatabaseContext _context;

        public SqlWarehouseRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Create(Hop hop)
        {
            _context.Hops.Add(hop);
            _context.SaveChanges();
            return hop.Id;
        }

        public void Delete(int id)
        {
            var hop = _context.Hops.FirstOrDefault(h => h.Id == id);
            _context.Hops.Remove(hop);
            _context.SaveChanges();
        }

        public void Update(Hop hop)
        {
            _context.Hops.Update(hop);
            _context.SaveChanges();
        }

        public Hop GetHopById(int id)
        {
            var hop = _context.Hops.FirstOrDefault(h => h.Id == id);

            return hop;
        }

        public Warehouse GetWarehouseByCode(string code)
        {
            var warehouse = _context.Warehouses.FirstOrDefault(w => w.Code == code);

            return warehouse;
        }

        public Warehouse GetRootWarehouse()
        {
            _context.Warehouses.Load();
            _context.WarehouseNextHops.Load();
            _context.Trucks.Load();
            _context.TransferWarehouses.Load();
            return _context.Hops.OfType<Warehouse>().Include(wh => wh.NextHops).FirstOrDefault(w => w.Id == 1);
        }

        public Hop GetHopByCode(string code)
        {
            var hop = _context.Hops.FirstOrDefault(h => h.Code == code);

            return hop;
        }

        public TransferWarehouse GetTransferWarehouseByCode(string code)
        {
            var transferWarehouse = _context.TransferWarehouses.FirstOrDefault(h => h.Code == code);
            
            return transferWarehouse;
        }

        public IEnumerable<Hop> GetAllHops()
        {
            return _context.Hops;
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return _context.Hops.Where(x => x.HopType == "Truck").Cast<Truck>();
        }

        public IEnumerable<WarehouseNextHops> GetAllWarehouseNextHops()
        {
            return _context.WarehouseNextHops;
        }

        public IEnumerable<Warehouse> GetAllWarehouses()
        {
            _context.Warehouses.Load();
            _context.WarehouseNextHops.Load();
            _context.Trucks.Load();
            _context.TransferWarehouses.Load();

            return _context.Warehouses.Include(wh => wh.NextHops);
        }
    }
}
