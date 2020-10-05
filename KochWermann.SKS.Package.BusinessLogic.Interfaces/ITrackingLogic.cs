using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Interfaces
{
    public interface ITrackingLogic
    {
        /// <summary>
        /// Transfer an existing parcel into the system from the service of a logistics partner.
        /// </summary>
        Parcel TransitionParcel(string trackingId);

        /// <summary>
        /// Get a certain parcel by tracking ID.
        /// </summary>
        Parcel GetParcel(string trackingId);

        /// <summary>
        /// Submit a new parcel.
        /// </summary>
        void SubmitParcel(Parcel parcel);

        /// <summary>
        /// Report that a parcel has been delivered at it's final destination address.
        /// </summary>
        void ReportParcelDelivery(string trackingId);

        /// <summary>
        /// Report that a parcel has arrived at a certain hop either Warehouse or Truck.
        /// </summary>
        void ReportParcelHop(string trackingId, string code);
    }    
} 