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

        private DAL_Exception ExceptionHandler(string message, Exception inner)
        {
            _logger.LogError(inner.ToString());
            return new DAL_Exception(message, inner);
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
            catch (SqlException ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
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
                var hop = _context.Hops.FirstOrDefault(h => h.Code == code);
                _context.Hops.Remove(hop);
                _context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DAL_NotFound_Exception($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (SqlException ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
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
            catch (SqlException ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
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
                return _context.Warehouses.Single(x => x.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DAL_NotFound_Exception($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
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
                return _context.Hops.OfType<Warehouse>().Include(wh => wh.NextHops).FirstOrDefault(w => w.IsRootWarehouse);
            }
            catch (SqlException ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
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
                return _context.Hops.Single(x => x.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DAL_NotFound_Exception($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
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
            return _context.Hops.Where(x => x.HopType == "Truck").AsEnumerable().Cast<Truck>();
        }

        public IEnumerable<WarehouseNextHops> GetAllWarehouseNextHops()
        {
            return _context.WarehouseNextHops.AsEnumerable();
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
            catch (SqlException ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }
    }
}
