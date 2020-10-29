using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;
using BlEntities = KochWermann.SKS.Package.BusinessLogic.Entities;
using DALEntities = KochWermann.SKS.Package.DataAccess.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DalMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public DalMapperProfile()
        {
            CreateMap<string, Geometry>().ConvertUsing<GeoJsonToGeometryConverter>();
            CreateMap<Geometry, string>().ConvertUsing<GeometryToGeoJsonConverter>();
            CreateMap<BlEntities.GeoCoordinate, Point>().ConvertUsing<GeoCoordinatesToPointConverter>();
            CreateMap<Point, BlEntities.GeoCoordinate>().ConvertUsing<PointToGeoCoordinatesConverter>();


            this.CreateMap<BlEntities.Hop, DALEntities.Hop>()
            .ForMember(dest => dest.LocationCoordinates, opt => opt.MapFrom(src => src.LocationCoordinates))
            .Include<BlEntities.Truck, DALEntities.Truck>()
            .Include<BlEntities.Warehouse, DALEntities.Warehouse>()
            .Include<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
               

            this.CreateMap<DALEntities.Hop, BlEntities.Hop>()
            .ForMember(dest => dest.LocationCoordinates, opt => opt.MapFrom(src => src.LocationCoordinates))
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