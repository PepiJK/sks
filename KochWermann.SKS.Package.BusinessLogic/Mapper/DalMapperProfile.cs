using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

            this.CreateMap<BlEntities.Hop, DALEntities.Hop>()
            .ForMember(dest => dest.LocationCoordinates, opt => opt.ConvertUsing(new PointConverter(), src => src.LocationCoordinates))
            .Include<BlEntities.Truck, DALEntities.Truck>()
            .Include<BlEntities.Warehouse, DALEntities.Warehouse>()
            .Include<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>();


            this.CreateMap<DALEntities.Hop, BlEntities.Hop>()
            .ForMember(dest => dest.LocationCoordinates, opt => opt.ConvertUsing(new PointConverter(), src => src.LocationCoordinates))
            .Include<DALEntities.Truck, BlEntities.Truck>()
            .Include<DALEntities.Warehouse, BlEntities.Warehouse>()
            .Include<DALEntities.TransferWarehouse, BlEntities.TransferWarehouse>();


            this.CreateMap<BlEntities.Warehouse, DALEntities.Warehouse>().ReverseMap();
            this.CreateMap<BlEntities.Truck, DALEntities.Truck>()
            .ForMember(dest => dest.RegionGeometry, opt => opt.MapFrom(src => src.RegionGeoJson))
            .ReverseMap();

            this.CreateMap<BlEntities.TransferWarehouse, DALEntities.TransferWarehouse>()
            .ForMember(dest => dest.RegionGeometry, opt => opt.MapFrom(src => src.RegionGeoJson))
            .ReverseMap();

            this.CreateMap<BlEntities.Warehouse, DALEntities.Warehouse>().ReverseMap();
            this.CreateMap<BlEntities.WarehouseNextHops, DALEntities.WarehouseNextHops>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            this.CreateMap<BlEntities.HopArrival, DALEntities.HopArrival>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            this.CreateMap<BlEntities.Recipient, DALEntities.Recipient>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            this.CreateMap<BlEntities.Parcel, DALEntities.Parcel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HopArrivals, opt => opt.MapFrom(src => CombineLists<BlEntities.HopArrival>(src.VisitedHops, src.FutureHops)));

            this.CreateMap<DALEntities.Parcel, BlEntities.Parcel>()
                .ForMember(dest => dest.VisitedHops, opt => opt.MapFrom(src => src.HopArrivals.Where(h => h.DateTime <= DateTime.Now)))
                .ForMember(dest => dest.FutureHops, opt => opt.MapFrom(src => src.HopArrivals.Where(h => h.DateTime > DateTime.Now)));
        }

        private List<T> CombineLists<T>(List<T> firstList, List<T> secondList)
        {
            if (firstList == null) return secondList;
            firstList.AddRange(secondList);
            return firstList;
        }
    }
}