using System;
using AutoMapper;
using Newtonsoft.Json;
using NetTopologySuite.Geometries;
using NetTopologySuite.Features;
using System.IO;
using System.Text;
using NetTopologySuite.IO;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Data;

namespace KochWermann.SKS.Package.BusinessLogic.Mapper
{
    [ExcludeFromCodeCoverage]
    public class PointConverter : IValueConverter<GeoCoordinate, Point>, IValueConverter<Point, GeoCoordinate>
    {
        private ResolutionContext _context;

        Point IValueConverter<GeoCoordinate, Point>.Convert(GeoCoordinate src, ResolutionContext context)
        {
            _context = context;
            if (src.Lat == null || src.Lon == null)
            {
                throw new NoNullAllowedException();
            }

            return new Point((double) src.Lon, (double) src.Lat) { SRID = 4326 };
        }


        GeoCoordinate IValueConverter<Point, GeoCoordinate>.Convert(Point src, ResolutionContext context)
        {
            _context = context;
            return new GeoCoordinate{Lat = src.X, Lon = src.Y};
        }
    }


    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GeoJsonToGeometryConverter : ITypeConverter<string, Geometry>
    {
        /// <summary>
        ///
        /// </summary>
        public Geometry Convert(string source, Geometry destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            var serializer = GeoJsonSerializer.CreateDefault();
            return ((Feature)serializer.Deserialize(new StringReader(source), typeof(Feature))).Geometry;
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GeometryToGeoJsonConverter : ITypeConverter<Geometry, string>
    {
        /// <summary>
        ///
        /// </summary>
        public string Convert(Geometry source, string destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            var serializer = GeoJsonSerializer.CreateDefault();
            var writer = new StringWriter();
            serializer.Serialize(writer, source, typeof(Feature));
            return writer.ToString();
        }
    }
}