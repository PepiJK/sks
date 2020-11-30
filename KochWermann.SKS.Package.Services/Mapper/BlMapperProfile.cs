using System.Net.Security;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;

using ServiceEntities = KochWermann.SKS.Package.Services.DTOs;
using BlEntities = KochWermann.SKS.Package.BusinessLogic.Entities;
using DALEntities = KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.Services.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BlMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public BlMapperProfile()
        {
            //ServiceLayer <=> BL
            this.CreateMap<ServiceEntities.Hop, BlEntities.Hop>()
            .Include<ServiceEntities.Truck, BlEntities.Truck>()
            .Include<ServiceEntities.Warehouse, BlEntities.Warehouse>()
            .Include<ServiceEntities.Transferwarehouse, BlEntities.TransferWarehouse>();

            this.CreateMap<BlEntities.Hop, ServiceEntities.Hop>()
            .Include<BlEntities.Truck, ServiceEntities.Truck>()
            .Include<BlEntities.Warehouse, ServiceEntities.Warehouse>()
            .Include<BlEntities.TransferWarehouse, ServiceEntities.Transferwarehouse>();

            this.CreateMap<ServiceEntities.Warehouse, BlEntities.Warehouse>().ReverseMap();
            this.CreateMap<ServiceEntities.Truck, BlEntities.Truck>().ReverseMap();
            this.CreateMap<ServiceEntities.Transferwarehouse, BlEntities.TransferWarehouse>().ReverseMap();
            this.CreateMap<ServiceEntities.Warehouse, BlEntities.Warehouse>().ReverseMap();
            this.CreateMap<ServiceEntities.WarehouseNextHops, BlEntities.WarehouseNextHops>().ReverseMap();
            this.CreateMap<ServiceEntities.HopArrival, BlEntities.HopArrival>().ReverseMap();
            this.CreateMap<ServiceEntities.Recipient, BlEntities.Recipient>().ReverseMap();
            this.CreateMap<ServiceEntities.GeoCoordinate, BlEntities.GeoCoordinate>().ReverseMap();

            this.CreateMap<ServiceEntities.Parcel, BlEntities.Parcel>();
            this.CreateMap<ServiceEntities.TrackingInformation, BlEntities.Parcel>();
            this.CreateMap<ServiceEntities.NewParcelInfo, BlEntities.Parcel>();
            this.CreateMap<BlEntities.Parcel, ServiceEntities.Parcel>();
            this.CreateMap<BlEntities.Parcel, ServiceEntities.TrackingInformation>();
            this.CreateMap<BlEntities.Parcel, ServiceEntities.NewParcelInfo>();

            this.CreateMap<ServiceEntities.WebhookResponse, BlEntities.WebhookResponse>().ReverseMap();
            this.CreateMap<ServiceEntities.WebhookMessage, BlEntities.WebhookMessage>().ReverseMap();
        }
    }
}