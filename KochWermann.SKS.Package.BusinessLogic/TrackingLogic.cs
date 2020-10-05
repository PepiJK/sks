using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        public Parcel TransitionParcel(string trackingId)
        {
            throw new System.NotImplementedException();
        }

        public Parcel GetParcel(string trackingId)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitParcel(Parcel parcel)
        {
            throw new System.NotImplementedException();
        }

        public void ReportParcelDelivery(string trackingId)
        {
            throw new System.NotImplementedException();
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            throw new System.NotImplementedException();
        }       
    }
}