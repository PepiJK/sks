using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private readonly IDatabaseContext _context;

        public SqlParcelRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Create(Parcel parcel)
        {
            _context.Parcels.Add(parcel);
            _context.SaveChanges();
            return parcel.Id;
        }

        public void Update(Parcel parcel)
        {
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var parcel = _context.Parcels.FirstOrDefault(x => x.Id == id);
            _context.Parcels.Remove(parcel);
            _context.SaveChanges();
        }

        public IEnumerable<Parcel> GetParcelByRecipient(Recipient recipient)
        {
            IEnumerable<Parcel> parcelList = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.VisitedHops)
                .Include(parcel => parcel.FutureHops)
                .Where(e => e.Recipient.Id == recipient.Id);

            return parcelList;
        }

        public Parcel GetParcelById(int id)
        {
            var parcel = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.VisitedHops)
                .Include(parcel => parcel.FutureHops)
                .FirstOrDefault(e => e.Id == id);
            return parcel;
        }

        public Parcel GetParcelByTrackingId(string trackingid)
        {
            var parcel = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.VisitedHops)
                .Include(parcel => parcel.FutureHops)
                .FirstOrDefault(e => e.TrackingId == trackingid);

            return parcel;
        }

        public IEnumerable<Parcel> GetAllParcels()
        {
            return _context.Parcels;
        }
    }
}
