using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class GeoCoordinate
    { 
        /// <summary>
        /// Latitude of the coordinate.
        /// </summary>
        /// <value>Latitude of the coordinate.</value>
        public double? Lat { get; set; }

        /// <summary>
        /// Longitude of the coordinate.
        /// </summary>
        /// <value>Longitude of the coordinate.</value>
        public double? Lon { get; set; }
    }
}
