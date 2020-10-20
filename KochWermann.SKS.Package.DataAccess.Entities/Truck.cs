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
    public partial class Truck : Hop
    { 
        /// <summary>
        /// Geometry instead of RegionGeoJson
        /// </summary>
        [Required]
        [Column(TypeName = "Geometry")]
        public Geometry RegionGeometry { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        [Required]
        public string NumberPlate { get; set; }
    }
}
