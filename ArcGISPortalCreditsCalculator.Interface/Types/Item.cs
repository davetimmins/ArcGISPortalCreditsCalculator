using ArcGIS.ServiceModel.Common;
using ArcGIS.ServiceModel.Operation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArcGISPortalCreditsCalculator.Interface.Types
{
    public class Item : PortalResponse, IRuleCollection, IEndpoint
    {
        public const String UrlTemplate = "content/items/{0}?f=json";

        public Item(String itemId)
        {
            if (String.IsNullOrWhiteSpace(itemId))
                throw new ArgumentNullException("itemId");
            Id = itemId;
        }
        
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("size")]
        public long SizeInBytes { get; set; }               

        public IEnumerable<IRule> Rules { get; set; }

        public string BuildAbsoluteUrl(string rootUrl)
        {
            return !RelativeUrl.Contains(rootUrl.Substring(6)) && !RelativeUrl.Contains(rootUrl.Substring(6))
                       ? rootUrl + RelativeUrl
                       : RelativeUrl;
        }

        public string RelativeUrl
        {
            get { return String.Format(UrlTemplate, Id); }
        }
    }

    public static class ItemTypes
    {
        public const String MapService = "Map Service";
        public const String FeatureService = "Feature Service";
        public const String GeocodingService = "Geocoding Service";
        public const String WebMap = "Web Map";
        public const String NetworkAnalysis = "Network Analysis Service";
        // TODO : add all the types available
    }
}
