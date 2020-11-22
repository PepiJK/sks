
using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class HopArrival
    { 
        [Required]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [Required]
        [ForeignKey("Hop")]
        public string Code { get; set; }

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        public string Description { get; set; }

        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        public DateTime? DateTime { get; set; }

        public int? VisitedParcelId { get; set; }
        public Parcel VisitedParcel { get; set; }

        public int? FutureParcelId { get; set; }
        public Parcel FutureParcel { get; set; }
        public Hop Hop { get; set; }
    }
}
