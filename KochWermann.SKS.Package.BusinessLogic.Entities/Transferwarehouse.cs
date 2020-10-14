using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{ 
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class TransferWarehouse : Hop
    { 
        /// <summary>
        /// GeoJSON of the are covered by the logistics partner.
        /// </summary>
        /// <value>GeoJSON of the are covered by the logistics partner.</value>
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        public string LogisticsPartner { get; set; }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        public string LogisticsPartnerUrl { get; set; }
    }
}
