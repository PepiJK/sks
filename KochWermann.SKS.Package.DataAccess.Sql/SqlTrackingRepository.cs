using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : ITrackingRepository
    {
        private readonly DatabaseContext _context;

        public SqlParcelRepository(DatabaseContext context)
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
            var entity = _context.Parcels.FirstOrDefault(item => item.TrackingId == parcel.TrackingId);

            if (entity != null)
            {
                entity.Recipient = parcel.Recipient;
                entity.Sender = parcel.Sender;
                entity.Weight = parcel.Weight;
                entity.State = parcel.State;
                entity.VisitedHops = parcel.VisitedHops;
                entity.FutureHops = parcel.FutureHops;

                _context.Parcels.Update(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {

            _context.Remove(_context.Parcels.Single(x => x.Id == id));
            _context.SaveChanges();
        }

        public IEnumerable<Parcel> GetParcelByRecipient(Recipient recipient)
        {
            IEnumerable<Parcel> parcelList = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.VisitedHops)
                .Include(parcel => parcel.FutureHops)
                .Where(e => e.Recipient == recipient);

            return parcelList;
        }

        public Parcel GetParcelById(int id)
        {
            var parcel = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.VisitedHops)
                .Include(parcel => parcel.FutureHops)
                .SingleOrDefault(e => e.Id == id);
            return parcel;
        }

        public Parcel GetParcelByTrackingId(string trackingid)
        {
            var parcel = _context.Parcels
                .Include(parcel => parcel.Recipient)
                .Include(parcel => parcel.Sender)
                .Include(parcel => parcel.VisitedHops)
                .Include(parcel => parcel.FutureHops)
                .SingleOrDefault(e => e.TrackingId == trackingid);

            return parcel;
        }

        public IEnumerable<Parcel> GetAllParcels()
        {
            return _context.Parcels.AsEnumerable();
        }
    }
}
