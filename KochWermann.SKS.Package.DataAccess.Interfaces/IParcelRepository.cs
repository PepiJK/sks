using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        int Create(Parcel parcel);
        void Update(Parcel parcel);
        void Delete(int id);

        Parcel GetParcelById(int id);
        Parcel GetParcelByTrackingId(string trackingid);
        IEnumerable<Parcel> GetAllParcels ();
        bool ContainsTrackingId(string trackingId);
    }
}