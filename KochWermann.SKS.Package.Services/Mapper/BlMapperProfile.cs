using ServiceEntities = KochWermann.SKS.Package.Services.DTOs;
using BlEntities = KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.Services.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class BlMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public BlMapperProfile()
        {
            this.CreateMap<ServiceEntities.Hop, BlEntities.Hop>()
            .Include<ServiceEntities.Truck, BlEntities.Truck>()
            .Include<ServiceEntities.Warehouse, BlEntities.Warehouse>()
            .Include<ServiceEntities.Transferwarehouse, BlEntities.Transferwarehouse>();

            this.CreateMap<BlEntities.Hop, ServiceEntities.Hop>()
            .Include<BlEntities.Truck, ServiceEntities.Truck>()
            .Include<BlEntities.Warehouse, ServiceEntities.Warehouse>()
            .Include<BlEntities.Transferwarehouse, ServiceEntities.Transferwarehouse>();


            this.CreateMap<ServiceEntities.Warehouse, BlEntities.Warehouse>().ReverseMap();
            this.CreateMap<ServiceEntities.Truck, BlEntities.Truck>().ReverseMap();
            this.CreateMap<ServiceEntities.Transferwarehouse, BlEntities.Transferwarehouse>().ReverseMap();
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
        }
    }
}