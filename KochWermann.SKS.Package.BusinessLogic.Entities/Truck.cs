namespace KochWermann.SKS.Package.BusinessLogic.Entities
{ 
    /// <summary>
    /// 
    /// </summary>
    public partial class Truck : Hop
    { 
        /// <summary>
        /// GeoJSON of the are covered by the truck.
        /// </summary>
        /// <value>GeoJSON of the are covered by the truck.</value>
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        public string NumberPlate { get; set; }
    }
}
