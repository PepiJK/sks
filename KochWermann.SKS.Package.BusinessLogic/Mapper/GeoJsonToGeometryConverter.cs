using System;
using AutoMapper;
using Newtonsoft.Json;
using NetTopologySuite.Geometries;
using NetTopologySuite.Features;
using System.IO;
using System.Text;
using NetTopologySuite.IO;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Mapper
{
    public class GeoCoordinatesToPointConverter : ITypeConverter<KochWermann.SKS.Package.BusinessLogic.Entities.GeoCoordinate, NetTopologySuite.Geometries.Point>
    {
        public Point Convert(GeoCoordinate source, Point destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            destination.X = System.Convert.ToDouble(source.Lon);
            destination.Y = System.Convert.ToDouble(source.Lat);

            return destination;
        }
    }

    public class PointToGeoCoordinatesConverter : ITypeConverter<NetTopologySuite.Geometries.Point, KochWermann.SKS.Package.BusinessLogic.Entities.GeoCoordinate>
    {
        public GeoCoordinate Convert(Point source, GeoCoordinate destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            destination.Lon = source.X;
            destination.Lat = source.Y;

            return destination;
        }
    }

    /// <summary>
    ///
    /// </summary>
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