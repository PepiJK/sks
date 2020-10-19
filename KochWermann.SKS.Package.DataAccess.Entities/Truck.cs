using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
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
        /// GeoJSON of the are covered by the truck.
        /// </summary>
        /// <value>GeoJSON of the are covered by the truck.</value>
        [Required]
        public string RegionGeoJson { get; set; }
        // [Column(TypeName = "Geometry")]
        // public Geometry RegionGeometry { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        [Required]
        public string NumberPlate { get; set; }
    }
}
