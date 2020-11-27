using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Data.SqlClient;
using NetTopologySuite;
using NetTopologySuite.Geometries;

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
                _logger.LogError($"Error creating new hop {ex}");
                throw new DALException("Error creating new hop", ex);
            }
        }

        public void Delete(string code)
        {
            try
            {
                var hop = GetHopByCode(code);

                _context.Hops.Remove(hop);
                _context.SaveChanges();
            }
            catch (DALNotFoundException ex)
            {
                _logger.LogError($"Could not find exactly one warehouse with code {code} {ex}");
                throw new DALNotFoundException($"Could not find exactly one warehouse with code {code}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting hop {ex}");
                throw new DALException("Error deleting hop", ex);
            }
        }

        public void Update(Hop hop)
        {
            try
            {
                _context.Hops.Update(hop);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating hop {ex}");
                throw new DALException("Error updating hop", ex);
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
                _logger.LogError($"Could not find exactly one warehouse with code {code} {ex}");
                throw new DALNotFoundException($"Could not find exactly one warehouse with code {code}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWarehouseByCode {ex}");
                throw new DALException("Error in GetWarehouseByCode", ex);
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

                return _context.Hops.OfType<Warehouse>()
                    .Include(wh => wh.NextHops)
                    .Single(w => w.IsRootWarehouse);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Could not find exactly one root warehouse {ex}");
                throw new DALNotFoundException($"Could not find exactly one root warehouse", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetRootWarehouse {ex}");
                throw new DALException("Error in GetRootWarehouse", ex);
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
                _logger.LogError($"Could not find exactly one warehouse with code {code} {ex}");
                throw new DALNotFoundException($"Could not find exactly one warehouse with code {code}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHopByCode {ex}");
                throw new DALException("Error in GetHopByCode", ex);
            }
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            try
            {
                return _context.Trucks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllTrucks {ex}");
                throw new DALException("Error in GetAllTrucks", ex);
            }
        }

        public IEnumerable<Warehouse> GetAllWarehouses()
        {
            try
            {
                _context.Warehouses.Load();
                _context.WarehouseNextHops.Load();
                _context.Trucks.Load();
                _context.TransferWarehouses.Load();

                return _context.Hops.OfType<Warehouse>().Include(wh => wh.NextHops);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllWarehouses {ex}");
                throw new DALException("Error in GetAllWarehouses", ex);
            }
        }

        public Hop GetHopByCoordinates (double longitude, double latitude)
        {
            try
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var coordinate = new Coordinate(latitude, longitude);
                var point = geometryFactory.CreatePoint(coordinate);

                var trucks = _context.Trucks.FirstOrDefault(x => x.RegionGeometry.Contains(point));
                var transferWarehouses = _context.TransferWarehouses.FirstOrDefault(x => x.RegionGeometry.Contains(point));

                if (trucks != null) return trucks;
                return transferWarehouses;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHopByCoordinates {ex}");
                throw new DALException("Error in GetHopByCoordinates", ex);
            }
        }

        public void ClearAllTables()
        {
            try
            {
                _logger.LogInformation("Clearing the existing DB (the entire one)");

                _context.HopArrivals.RemoveRange(_context.HopArrivals);
                _context.WarehouseNextHops.RemoveRange(_context.WarehouseNextHops);
                _context.Hops.RemoveRange(_context.Hops);
                _context.Parcels.RemoveRange(_context.Parcels);
                _context.Recipients.RemoveRange(_context.Recipients);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ClearAllTables {ex}");
                throw new DALException("Error in ClearAllTables", ex);
            }
        }
    }
}
