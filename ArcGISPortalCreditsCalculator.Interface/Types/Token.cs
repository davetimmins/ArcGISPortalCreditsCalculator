using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISPortalCreditsCalculator.Interface.Types
{
    public class Token : PortalObject
    {
        public const String UrlTemplate = "generateToken";

        [JsonProperty("token")]
        public String Value { get; set; }

        [JsonProperty("expires")]
        public long Expiry { get; set; }

        public bool IsExpired
        {
            get { return !String.IsNullOrWhiteSpace(Value) && Expiry > 0 && DateTime.Compare(new DateTime(Expiry), DateTime.UtcNow) > 0; }
        }

        [JsonProperty("ssl")]
        public String AlwaysUseSsl { get; set; }
        
        public static Dictionary<String, String> DefaultParameters
        {
            get
            {
                var postData = new Dictionary<string, string>();
                postData.Add("f", "json");
                postData.Add("client", "referer");
                postData.Add("referer", "requestip");
                return postData;
            }
        }
    }
}
