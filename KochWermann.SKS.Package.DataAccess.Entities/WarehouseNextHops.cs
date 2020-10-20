using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace KochWermann.SKS.Package.DataAccess.Entities
{ 
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class WarehouseNextHops
    { 
        [Required]
        [Key]
        public int Id {get; set;}

        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        public int? TraveltimeMins { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        public Hop Hop { get; set; }
    }
}
