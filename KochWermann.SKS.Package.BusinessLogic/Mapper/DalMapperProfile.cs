using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;
using BlEntities = KochWermann.SKS.Package.BusinessLogic.Entities;
using DALEntities = KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class DalMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
        public DalMapperProfile()
        {
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