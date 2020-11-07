using Microsoft.EntityFrameworkCore;
using KochWermann.SKS.Package.DataAccess.Entities;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<Hop>().HasDiscriminator(h => h.HopType);
            builder.Entity<Truck>().HasBaseType<Hop>();
            builder.Entity<Warehouse>().HasBaseType<Hop>();
            builder.Entity<TransferWarehouse>().HasBaseType<Hop>();
        }

        public virtual DbSet<Parcel> Parcels { get; set; }
        public virtual DbSet<Hop> Hops { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<Truck> Trucks { get; set; }
        public virtual DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        public virtual DbSet<TransferWarehouse> TransferWarehouses { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<HopArrival> HopArrivals { get; set; }
    }
}