using System;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    [DataContract]
    public partial class WebhookResponse
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [DataMember(Name="trackingId")]
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [DataMember(Name="url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name="created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
