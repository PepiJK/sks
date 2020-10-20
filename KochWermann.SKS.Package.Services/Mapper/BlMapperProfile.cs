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
    public class BlMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
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

            CreateMap<string, Geometry>().ConvertUsing<GeoJsonToGeometryConverter>();
            CreateMap<Geometry, string>().ConvertUsing<GeometryToGeoJsonConverter>();

            this.CreateMap<BlEntities.Hop, DALEntities.Hop>()
            .ForPath(dest => dest.LocationCoordinates.X, opt => opt.MapFrom(src => src.LocationCoordinates.Lat))
            .ForPath(dest => dest.LocationCoordinates.Y, opt => opt.MapFrom(src => src.LocationCoordinates.Lon))
            .Include<BlEntities.Truck, DALEntities.Truck>()
            .Include<BlEntities.Warehouse, DALEntities.Warehouse>()
            .Include<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
               

            this.CreateMap<DALEntities.Hop, BlEntities.Hop>()
            .ForPath(dest => dest.LocationCoordinates.Lat, opt => opt.MapFrom(src => src.LocationCoordinates.X))
            .ForPath(dest => dest.LocationCoordinates.Lon, opt => opt.MapFrom(src => src.LocationCoordinates.Y))
            .Include<DALEntities.Truck, BlEntities.Truck>()
            .Include<DALEntities.Warehouse, BlEntities.Warehouse>()
            .Include<DALEntities.TransferWarehouse, BlEntities.TransferWarehouse>();
                

            this.CreateMap<BlEntities.Warehouse, DALEntities.Warehouse>().ReverseMap();
            this.CreateMap<BlEntities.Truck, DALEntities.Truck>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegionGeometry, opt => opt.MapFrom(src => src.RegionGeoJson))
            .ReverseMap();

            this.CreateMap<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegionGeometry, opt => opt.MapFrom(src => src.RegionGeoJson))
            .ReverseMap();

            this.CreateMap<BlEntities.Warehouse, DALEntities.Warehouse>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            this.CreateMap<BlEntities.WarehouseNextHops, DALEntities.WarehouseNextHops>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            this.CreateMap<BlEntities.HopArrival, DALEntities.HopArrival>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            this.CreateMap<BlEntities.Recipient, DALEntities.Recipient>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            this.CreateMap<BlEntities.Parcel, DALEntities.Parcel>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
        }
    }
}