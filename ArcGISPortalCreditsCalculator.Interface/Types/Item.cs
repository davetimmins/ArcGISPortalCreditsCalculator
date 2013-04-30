using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISPortalCreditsCalculator.Interface.Types
{
    public class Item : PortalObject, IRuleCollection
    {
        public const String UrlTemplate = "content/items/{0}?f=json";

        public Item()
        {

        }

        public Item(String itemId)
        {
            if (String.IsNullOrWhiteSpace(itemId))
                throw new ArgumentNullException("itemId");
            Id = itemId;
        }

        public String Url { get { return String.Format(UrlTemplate, Id); } }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("size")]
        public long SizeInBytes { get; set; }               

        public IEnumerable<IRule> Rules { get; set; }
    }

    public static class ItemTypes
    {
        public const String FeatureService = "Feature Service";
        public const String GeocodingService = "Geocoding Service";
        public const String WebMap = "Web Map";
        public const String NetworkAnalysis = "Network Analysis Service";
        // TODO : add all the types available
    }
}
