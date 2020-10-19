using Microsoft.EntityFrameworkCore;
using KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        {
            this.Database.EnsureCreated();
        }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Hop> Hops { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        public DbSet<TransferWarehouse> TransferWarehouses { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<HopArrival> HopArrivals { get; set; }
    }
}