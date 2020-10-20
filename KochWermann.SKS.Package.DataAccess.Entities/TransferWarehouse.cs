using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace KochWermann.SKS.Package.DataAccess.Entities
{ 
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class TransferWarehouse : Hop
    { 
        /// <summary>
        /// Geometry instead of RegionGeoJson
        /// </summary>
        [Required]
        [Column(TypeName = "Geometry")]
        public Geometry RegionGeometry { get; set; }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        [Required]
        public string LogisticsPartner { get; set; }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        public string LogisticsPartnerUrl { get; set; }
    }
}
