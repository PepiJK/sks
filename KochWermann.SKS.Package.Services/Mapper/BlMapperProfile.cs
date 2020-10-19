using ServiceEntities = KochWermann.SKS.Package.Services.DTOs;
using BlEntities = KochWermann.SKS.Package.BusinessLogic.Entities;
using DALEntities = KochWermann.SKS.Package.DataAccess.Entities;

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

            //BL <=> DAL
            this.CreateMap<BlEntities.Hop, DALEntities.Hop>()
            .Include<BlEntities.Truck, DALEntities.Truck>()
            .Include<BlEntities.Warehouse, DALEntities.Warehouse>()
            .Include<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>();

            this.CreateMap<DALEntities.Hop, BlEntities.Hop>()
            .Include<DALEntities.Truck, BlEntities.Truck>()
            .Include<DALEntities.Warehouse, BlEntities.Warehouse>()
            .Include<DALEntities.TransferWarehouse, BlEntities.TransferWarehouse>();


            this.CreateMap<BlEntities.Warehouse, DALEntities.Warehouse>().ReverseMap();
            this.CreateMap<BlEntities.Truck, DALEntities.Truck>().ReverseMap();
            this.CreateMap<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>().ReverseMap();
            this.CreateMap<BlEntities.Warehouse, DALEntities.Warehouse>().ReverseMap();
            this.CreateMap<BlEntities.WarehouseNextHops, DALEntities.WarehouseNextHops>().ReverseMap();
            this.CreateMap<BlEntities.HopArrival, DALEntities.HopArrival>().ReverseMap();
            this.CreateMap<BlEntities.Recipient, DALEntities.Recipient>().ReverseMap();
            this.CreateMap<BlEntities.GeoCoordinate, DALEntities.GeoCoordinate>().ReverseMap();
            
            this.CreateMap<BlEntities.Parcel, DALEntities.Parcel>().ReverseMap();
        }
    }
}