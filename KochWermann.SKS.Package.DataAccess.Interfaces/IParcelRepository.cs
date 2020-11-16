using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        int Create(Parcel parcel);
        void Update(Parcel parcel);
        void Delete(int id);

        IEnumerable<Parcel> GetParcelByRecipient(Recipient recipient);
        Parcel GetParcelById(int id);
        Parcel GetParcelByTrackingId(string trackingid);
        IEnumerable<Parcel> GetAllParcels ();
        bool ContainsTrackingID(string trackingId);
    }
}