using System;
using KochWermann.SKS.Package.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IDatabaseContext : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService
    {
        int SaveChanges();
        DbSet<Parcel> Parcels { get; set; }
        DbSet<Hop> Hops { get; set; }
        DbSet<Warehouse> Warehouses { get; set; }
        DbSet<Truck> Trucks { get; set; }
        DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        DbSet<TransferWarehouse> TransferWarehouses { get; set; }
        DbSet<Recipient> Recipients { get; set; }
        DbSet<HopArrival> HopArrivals { get; set; }
    }
}