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

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<Hop>().HasDiscriminator(h => h.HopType);
            builder.Entity<Truck>().HasBaseType<Hop>();
            builder.Entity<Warehouse>().HasBaseType<Hop>();
            builder.Entity<TransferWarehouse>().HasBaseType<Hop>();
            builder.Entity<Warehouse>().HasMany(wh => wh.NextHops);
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