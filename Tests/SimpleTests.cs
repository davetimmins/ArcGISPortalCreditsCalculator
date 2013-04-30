using ArcGISPortalCreditsCalculator;
using ArcGISPortalCreditsCalculator.Interface;
using ArcGISPortalCreditsCalculator.Interface.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SimpleTests
    {
        [Fact]
        public async Task HydrateItemWorks()
        {
            var id = ""; // enter item id here

            var gateway = new PortalGateway("", ""); // use ago account credentials if required
            var items = await gateway.HydrateItems(new[] { id });

            Assert.NotNull(items);
            Assert.NotEmpty(items);
            Assert.Equal(items.Count, 1);
            Assert.Equal(id, items.First().Id);
            Assert.True(items.First().SizeInBytes > 0);
            Assert.NotNull(items.First().Type);
            Assert.NotNull(items.First().Rules);
            Assert.NotEmpty(items.First().Rules);
        }

        [Fact]
        public async Task CalculateCreditsUsesNumberOfOperations()
        {
            var id = ""; // enter item id here

            var gateway = new PortalGateway("", ""); // use ago account credentials if required
            var items = await gateway.HydrateItems(new[] { id });

            foreach (var item in items)
            {
                var ruleItem = item;
                Assert.Equal(0, gateway.CalculateCredits(ruleItem));
                foreach (var rule in ruleItem.Rules)
                {
                    rule.NumberOfOperations = 10;
                }
                Assert.True(gateway.CalculateCredits(ruleItem) > 0);
            }
        }

        [Fact]
        public void GeocodingRuleCorrect()
        {
            var rules = new AGOPortalRule { Rules = new List<IRule> { new GeocodingRule() { NumberOfOperations = 1000 } } };
            var map = new Dictionary<string, Func<IRuleCollection>> { { ItemTypes.GeocodingService, () => rules } };
            
            var gateway = new PortalGateway("", map);
            
            Assert.Equal(80, gateway.CalculateCredits(rules));
        }

        // TODO : add more tests
    }
}
