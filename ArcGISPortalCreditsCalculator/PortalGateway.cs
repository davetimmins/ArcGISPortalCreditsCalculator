using ArcGIS.ServiceModel.Logic;
using ArcGIS.ServiceModel.Operation;
using ArcGISPortalCreditsCalculator.Interface;
using ArcGISPortalCreditsCalculator.Interface.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcGISPortalCreditsCalculator
{
    public class PortalGateway : ArcGIS.ServiceModel.Logic.PortalGateway, IRuleEngine
    {
        readonly Dictionary<string, Func<IRuleCollection>> _ruleMap;

        public PortalGateway()
            : this("http://www.arcgis.com/sharing/rest", AGOPortalRule.RuleMap)
        { }

        public PortalGateway(String rootUrl)
            : this(rootUrl, AGOPortalRule.RuleMap)
        { }

        public PortalGateway(Dictionary<string, Func<IRuleCollection>> ruleMap)
            : this("http://www.arcgis.com/sharing/rest", ruleMap)
        { }

        public PortalGateway(String rootUrl, Dictionary<string, Func<IRuleCollection>> ruleMap)
            : base(rootUrl)
        {
            _ruleMap = ruleMap;
        }

        public async Task<List<Item>> HydrateItems(ICollection<String> itemIds)
        {
            var items = new List<Item>();

            foreach (var id in itemIds)
            {
                var item = await Get<Item>(new Item(id));
                if (_ruleMap.ContainsKey(item.Type))
                    item.Rules = _ruleMap[item.Type]().Rules;

                items.Add(item);
            }

            return items;
        }

        public double CalculateCredits(IRuleCollection rules)
        {
            return rules.Rules.Where(r => r.NumberOfOperations > 0)
                .Sum(rule => (rule.CreditsPerOperation * Math.Abs(rule.NumberOfOperations)));
        }
    }
}
