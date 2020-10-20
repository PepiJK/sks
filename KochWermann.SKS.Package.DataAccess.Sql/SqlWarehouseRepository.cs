using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
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

        public int Create(Hop hop)
        {
            _context.Hops.Add(hop);
            _context.SaveChanges();
            return hop.Id;
        }

        public void Delete(int id)
        {
            _context.Remove(_context.Hops.Single(x => x.Id == id));
            _context.SaveChanges();
        }

        public void Update(Hop hop)
        {
                var hopToUpdate = GetHopById(hop.Id);
                _context.Entry(hopToUpdate).CurrentValues.SetValues(hop);
                _context.SaveChanges();
        }

        public Hop GetHopById(int id)
        {
            return _context.Hops.Single(x => x.Id == id);
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
            return _context.Hops.OfType<Warehouse>().Include(wh => wh.NextHops).FirstOrDefault(x => x.Id == 1);
        }

        public Hop GetHopByCode(string code)
        {
            return _context.Hops.Single(x => x.Code == code);
        }

        public TransferWarehouse GetTransferWarehouseByCode(string code)
        {
            return _context.TransferWarehouses.Single(x => x.Code == code);
        }

        public IEnumerable<Hop> GetAllHops()
        {
            return _context.Hops;
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return _context.Hops.Where(x => x.HopType == "Truck").AsEnumerable().Cast<Truck>();
        }

        public IEnumerable<WarehouseNextHops> GetAllWarehouseNextHops()
        {
            return _context.Hops.AsEnumerable().Cast<WarehouseNextHops>();
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
