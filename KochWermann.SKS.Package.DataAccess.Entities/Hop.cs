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
    public partial class Hop
    { 
        /// <summary>
        /// Gets or Sets HopType
        /// </summary>
        [Required]     
        public string HopType { get; set; }

        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [Required]
        [Key]
        public string Code { get; set; }

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        public string Description { get; set; }

        /// <summary>
        /// Delay processing takes on the hop.
        /// </summary>
        /// <value>Delay processing takes on the hop.</value>
        public int? ProcessingDelayMins { get; set; }

        /// <summary>
        /// Name of the location (village, city, ..) of the hop.
        /// </summary>
        /// <value>Name of the location (village, city, ..) of the hop.</value>
        public string LocationName { get; set; }

        /// <summary>
        /// GeometryPoint instead of GeoCoordinate
        /// </summary>
        [Column(TypeName = "Geometry")]
        public Point LocationCoordinates { get; set; }
    }
}
