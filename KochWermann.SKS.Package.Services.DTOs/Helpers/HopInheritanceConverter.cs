using System;
using Newtonsoft.Json.Linq;

namespace KochWermann.SKS.Package.Services.DTOs.Helpers
{
    public class HopInheritanceConverter : JsonCreationConverter<Hop>
    {
        protected override Hop Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["hopType"] == null)
            {
                if (jObject["numberPlate"] != null && ((string)jObject["numberPlate"]) != "")
                {
                    var truck = new Truck();
                    truck.HopType = "Truck";
                    return truck;
                }
                else
                {
                    var warehouse = new Warehouse();
                    warehouse.HopType = "Warehouse";
                    return warehouse;
                }
            }
            else
            {
                if (jObject["hopType"].Type != JTokenType.String) throw new ArgumentOutOfRangeException("hopType must be of type string");

                string hoptype = jObject["hopType"].Value<string>().ToLower();

                if (hoptype == "warehouse")
                {
                    return new Warehouse();
                }
                else if (hoptype == "truck")
                {
                    return new Truck();
                }
                else if (hoptype == "transferwarehouse")
                {
                    return new Transferwarehouse();
                }
                else
                {
                    throw new Newtonsoft.Json.JsonSerializationException($"{hoptype} is not a valid subclass of class Hop!");
                }
            }
        }
    }
}