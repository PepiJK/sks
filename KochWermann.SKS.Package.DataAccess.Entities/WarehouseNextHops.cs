using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Entities
{ 
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class WarehouseNextHops
    { 
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        public int? TraveltimeMins { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        public Hop Hop { get; set; }
    }
}
