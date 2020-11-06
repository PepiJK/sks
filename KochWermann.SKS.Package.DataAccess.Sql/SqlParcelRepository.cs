using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

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

        private DAL_Exception ExceptionHandler(string message, Exception inner)
        {
            _logger.LogError(inner.ToString());
            return new DAL_Exception(message, inner);
        }

        public int Create(Parcel parcel)
        {
            try
            {
                _context.Parcels.Add(parcel);
                _context.SaveChanges();
                return parcel.Id;
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

        public void Update(Parcel parcel)
        {
            try
            {
                var p = GetParcelById(parcel.Id);
                _context.Entry(p).CurrentValues.SetValues(parcel);
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

        public void Delete(int id)
        {
            try
            {
                var parcel = _context.Parcels.FirstOrDefault(x => x.Id == id);
                _context.Parcels.Remove(parcel);
                _context.SaveChanges();
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

        public IEnumerable<Parcel> GetParcelByRecipient(Recipient recipient)
        {
            IEnumerable<Parcel> parcelList = _context.Parcels
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
                return _context.Parcels.FirstOrDefault(x => x.Id == id);
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

        public Parcel GetParcelByTrackingId(string trackingid)
        {
            try
            {
                var parcel = _context.Parcels
                    .Include(parcel => parcel.Recipient)
                    .Include(parcel => parcel.Sender)
                    .Include(parcel => parcel.HopArrivals)
                    .FirstOrDefault(x => x.TrackingId == trackingid);

                return parcel;
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

        public IEnumerable<Parcel> GetAllParcels()
        {
            return _context.Parcels.AsEnumerable();
        }
    }
}
