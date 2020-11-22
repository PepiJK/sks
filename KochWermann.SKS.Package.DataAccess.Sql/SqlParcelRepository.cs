using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SqlParcelRepository> _logger;


        public SqlParcelRepository(DatabaseContext context, ILogger<SqlParcelRepository> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogTrace("SqlParcelRepository created");
        }

        public int Create(Parcel parcel)
        {
            try
            {
                _context.Parcels.Add(parcel);
                _context.SaveChanges();
                return parcel.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating new parcel {ex}");
                throw new DALException("Error creating new parcel", ex);
            }
        }

        public void Update(Parcel parcel)
        {
            try
            {
                _context.Parcels.Update(parcel);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating parcel {ex}");
                throw new DALException("Error updating parcel", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var parcel = GetParcelById(id);

                _context.Parcels.Remove(parcel);
                _context.SaveChanges();
            }
            catch (DALNotFoundException ex)
            {
                _logger.LogError($"Could not find exactly one parcel with id {id} {ex}");
                throw new DALNotFoundException($"Could not find exactly one parcel with id {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting parcel {ex}");
                throw new DALException("Error deleting parcel", ex);
            }
        }

        public Parcel GetParcelById(int id)
        {
            try
            {
                return _context.Parcels
                    .Include(parcel => parcel.Recipient)
                    .Include(parcel => parcel.Sender)
                    .Include(parcel => parcel.VisitedHops)
                    .Include(parcel => parcel.FutureHops)
                    .Single(p => p.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Could not find exactly one parcel with id {id} {ex}");
                throw new DALNotFoundException($"Could not find exactly one parcel with id {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetParcelById {ex}");
                throw new DALException("Error in GetParcelById", ex);
            }
        }

        public Parcel GetParcelByTrackingId(string trackingid)
        {
            try
            {
                return _context.Parcels
                    .Include(parcel => parcel.Recipient)
                    .Include(parcel => parcel.Sender)
                    .Include(parcel => parcel.VisitedHops)
                    .Include(parcel => parcel.FutureHops)
                    .Single(x => x.TrackingId == trackingid);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Could not find exactly one parcel with trackingId {trackingid} {ex}");
                throw new DALNotFoundException($"Could not find exactly one parcel with trackingId {trackingid}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetParcelByTrackingId {ex}");
                throw new DALException("Error in GetParcelByTrackingId", ex);
            }
        }

        public IEnumerable<Parcel> GetAllParcels()
        {
            try
            {
                return _context.Parcels;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllParcels {ex}");
                throw new DALException("Error in GetAllParcels", ex);
            }
        }

        public bool ContainsTrackingId(string trackingId)
        {
            try
            {
                return _context.Parcels.Any(p => p.TrackingId == trackingId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ContainsTrackingId {ex}");
                throw new DALException("Error in ContainsTrackingId", ex);
            }
        }

    }
}
