using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Data.SqlClient;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlWarehouseRepository : IWarehouseRepository
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SqlWarehouseRepository> _logger;

        public SqlWarehouseRepository(DatabaseContext context, ILogger<SqlWarehouseRepository> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogTrace("SqlWarehouseRepository created");
        }

        public string Create(Hop hop)
        {
            try
            {
                if ((hop as Warehouse)?.Level == 0)
                {
                    (hop as Warehouse).IsRootWarehouse = true;
                }
                   
                _context.Hops.Add(hop);
                _context.SaveChanges();
                return hop.Code;
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void Delete(string code)
        {
            try
            {
                var hop = _context.Hops.First(h => h.Code == code);
                _context.Hops.Remove(hop);
                _context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DALNotFoundException($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void Update(Hop hop)
        {
            try
            {
                var h = GetHopByCode(hop.Code);
                _context.Entry(h).CurrentValues.SetValues(h);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public Warehouse GetWarehouseByCode(string code)
        {
            try
            {
                return _context.Warehouses.First(x => x.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DALNotFoundException($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public Warehouse GetRootWarehouse()
        {
            try
            {
                _context.Warehouses.Load();
                _context.WarehouseNextHops.Load();
                _context.Trucks.Load();
                _context.TransferWarehouses.Load();
                return _context.Hops.OfType<Warehouse>().Include(wh => wh.NextHops).First(w => w.IsRootWarehouse);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DALNotFoundException($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public Hop GetHopByCode(string code)
        {
            try
            {
                return _context.Hops.First(x => x.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DALNotFoundException($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public TransferWarehouse GetTransferWarehouseByCode(string code)
        {
            var transferWarehouse = _context.TransferWarehouses.First(h => h.Code == code);

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
            try
            {
                _context.Warehouses.Load();
                _context.WarehouseNextHops.Load();
                _context.Trucks.Load();
                _context.TransferWarehouses.Load();
                return _context.Warehouses.Include(wh => wh.NextHops);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        private DALException ExceptionHandler(string message, Exception inner)
        {
            _logger.LogError(inner.ToString());
            return new DALException(message, inner);
        }
    }
}
