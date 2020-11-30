using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class WebhookMessage
    { 
        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [DataMember(Name="trackingId")]
        public string TrackingId { get; set; }
        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        public enum StateEnum
        {
            /// <summary>
            /// Enum PickupEnum for Pickup
            /// </summary>
            [EnumMember(Value = "Pickup")]
            PickupEnum = 0,
            /// <summary>
            /// Enum InTransportEnum for InTransport
            /// </summary>
            [EnumMember(Value = "InTransport")]
            InTransportEnum = 1,
            /// <summary>
            /// Enum InTruckDeliveryEnum for InTruckDelivery
            /// </summary>
            [EnumMember(Value = "InTruckDelivery")]
            InTruckDeliveryEnum = 2,
            /// <summary>
            /// Enum TransferredEnum for Transferred
            /// </summary>
            [EnumMember(Value = "Transferred")]
            TransferredEnum = 3,
            /// <summary>
            /// Enum DeliveredEnum for Delivered
            /// </summary>
            [EnumMember(Value = "Delivered")]
            DeliveredEnum = 4
        }

        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        public StateEnum? State { get; set; }

        /// <summary>
        /// Hops visited in the past.
        /// </summary>
        /// <value>Hops visited in the past.</value>
        public List<HopArrival> VisitedHops { get; set; }

        /// <summary>
        /// Hops coming up in the future - their times are estimations.
        /// </summary>
        /// <value>Hops coming up in the future - their times are estimations.</value>
        public List<HopArrival> FutureHops { get; set; }
    }
}
