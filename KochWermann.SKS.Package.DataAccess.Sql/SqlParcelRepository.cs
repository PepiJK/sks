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
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void Update(Parcel parcel)
        {
            try
            {
                var p = GetParcelById(parcel.Id);
                _context.Entry(p).CurrentValues.SetValues(parcel);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var parcel = _context.Parcels.First(x => x.Id == id);
                _context.Parcels.Remove(parcel);
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

        public IEnumerable<Parcel> GetParcelByRecipient(Recipient recipient)
        {
            var parcelList = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.HopArrivals)
                .Where(e => e.Recipient.Id == recipient.Id);

            return parcelList;
        }

        public Parcel GetParcelById(int id)
        {
            try
            {
                return _context.Parcels.First(x => x.Id == id);
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

        public Parcel GetParcelByTrackingId(string trackingid)
        {
            try
            {
                var parcel = _context.Parcels
                    .Include(parcel => parcel.Recipient)
                    .Include(parcel => parcel.Sender)
                    .Include(parcel => parcel.HopArrivals)
                    .First(x => x.TrackingId == trackingid);

                return parcel;
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

        public IEnumerable<Parcel> GetAllParcels()
        {
            return _context.Parcels.AsEnumerable();
        }

        public bool ContainsTrackingID(string trackingId)
        {
            return _context.Parcels.Any(p => p.TrackingId == trackingId);
        }

        private DALException ExceptionHandler(string message, Exception inner)
        {
            _logger.LogError(inner.ToString());
            return new DALException(message, inner);
        }

    }
}
