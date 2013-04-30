using ArcGISPortalCreditsCalculator.Interface;
using ArcGISPortalCreditsCalculator.Interface.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISPortalCreditsCalculator
{
    public class PortalGateway : IPortalGateway, IRuleEngine
    {
        const String AGOPortalUrl = "http://www.arcgis.com/sharing/rest/";
        readonly String _username;
        readonly String _password;
        readonly Dictionary<string, Func<IRuleCollection>> _ruleMap;

        public PortalGateway()
            : this(AGOPortalUrl, String.Empty, String.Empty, AGOPortalRule.RuleMap)
        { }

        public PortalGateway(String rootUrl)
            : this(rootUrl, String.Empty, String.Empty, AGOPortalRule.RuleMap)
        { }

        public PortalGateway(String rootUrl, Dictionary<string, Func<IRuleCollection>> ruleMap)
            : this(rootUrl, String.Empty, String.Empty, ruleMap)
        { }

        public PortalGateway(String username, String password)
            : this(AGOPortalUrl, username, password, AGOPortalRule.RuleMap)
        { }

        public PortalGateway(String rootUrl, String username, String password, Dictionary<string, Func<IRuleCollection>> ruleMap)
        {
            if (!rootUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase)) rootUrl += "/";
            RootUrl = rootUrl;

            _ruleMap = ruleMap;
            _username = username;
            _password = password;
        }
        
        async Task<Token> CheckGenerateToken()
        {            
            if (String.IsNullOrWhiteSpace(_username) && String.IsNullOrWhiteSpace(_password)) return null;
            if (Token != null && !Token.IsExpired) return Token;

            Token = null;
            var parameters = Token.DefaultParameters;
            parameters.Add("username", _username);
            parameters.Add("password", _password);
            return await Post<Token>(Token.UrlTemplate, parameters);
        }               

        public string RootUrl { get; set; }

        public Token Token { get; set; }

        public async Task<List<Item>> HydrateItems(ICollection<String> itemIds)
        {
            var items = new List<Item>();

            foreach (var id in itemIds)
            {
                var item = await Get<Item>(String.Format(Item.UrlTemplate, id));
                if (_ruleMap.ContainsKey(item.Type))
                    item.Rules = _ruleMap[item.Type]().Rules;

                items.Add(item);
            }

            return items;
        }

        public double CalculateCredits(IRuleCollection rules)
        {
            double result = 0;

            foreach (var rule in rules.Rules.Where(r => r.NumberOfOperations > 0))
            {                
                result += (rule.CreditsPerOperation * Math.Abs(rule.NumberOfOperations)); // / rule.SizeModifier;
            }

            return result;
        }

        /// <summary>
        /// Perform a GET operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <remarks>need to use <see cref="PortalObject"/> as the response returns 200 even when an error occurs</remarks>
        /// <returns></returns>
        async Task<T> Get<T>(string url) where T : PortalObject
        {
            var token = await CheckGenerateToken();

            if (token != null && !String.IsNullOrWhiteSpace(token.Value))
                url += "&token=" + token.Value;

            url = RootUrl + url;

            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler))
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    if (result.Error != null)
                        throw new InvalidOperationException(result.Error.ExceptionMessage);

                    return result;
                }
            }
        }

        async Task<T> Post<T>(string url, Dictionary<string, string> parameters) where T : PortalObject
        {
            url = RootUrl.Replace("http://", "https://") + url;

            if (!parameters.ContainsKey("f"))
                parameters.Add("f", "json");
            if (!parameters.ContainsKey("token") && Token != null && !String.IsNullOrWhiteSpace(Token.Value))
                parameters.Add("token", Token.Value);

            HttpContent content = new FormUrlEncodedContent(parameters);
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler))
                {
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    if (result.Error != null)
                        throw new InvalidOperationException(result.Error.ExceptionMessage);

                    return result;
                }
            }
        }
    }
}
