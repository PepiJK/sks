using System.Collections.Generic;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Warehouse : Hop
    {
        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Next hops after this warehouse (warehouses or trucks).
        /// </summary>
        /// <value>Next hops after this warehouse (warehouses or trucks).</value>
        public List<WarehouseNextHops> NextHops { get; set; }
    }
}