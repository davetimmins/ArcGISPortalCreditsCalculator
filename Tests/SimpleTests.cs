using ArcGIS.ServiceModel.Logic;
using ArcGIS.ServiceModel.Operation;
using ArcGISPortalCreditsCalculator;
using ArcGISPortalCreditsCalculator.Interface;
using ArcGISPortalCreditsCalculator.Interface.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PortalGateway = ArcGISPortalCreditsCalculator.PortalGateway;

namespace Tests
{
    public class SimpleTests
    {
        [Fact]
        public async Task HydrateItemWorks()
        {
            var id = "50d0bf4c72ca40e7bdabf11133697244"; // enter item id here

            var gateway = new PortalGateway(); // use ago account credentials if required
            gateway.Serializer = new ServiceStackSerializer();

            var items = await gateway.HydrateItems(new[] { id });

            Assert.NotNull(items);
            Assert.NotEmpty(items);
            Assert.Equal(items.Count, 1);
            Assert.Equal(id, items.First().Id);
            Assert.NotNull(items.First().Type);
            Assert.NotNull(items.First().Rules);
            Assert.NotEmpty(items.First().Rules);
        }

        [Fact]
        public async Task CalculateCreditsUsesNumberOfOperations()
        {
            var id = "50d0bf4c72ca40e7bdabf11133697244"; // enter item id here

            var gateway = new PortalGateway(); // use ago account credentials if required
            gateway.Serializer = new ServiceStackSerializer();

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
            
            var gateway = new PortalGateway(map);
            gateway.Serializer = new ServiceStackSerializer();
            
            Assert.Equal(80, gateway.CalculateCredits(rules));
        }

        // TODO : add more tests
    }

    public class ServiceStackSerializer : ISerializer
    {
        public ServiceStackSerializer()
        {
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
            ServiceStack.Text.JsConfig.IncludeTypeInfo = false;
            ServiceStack.Text.JsConfig.ConvertObjectTypesIntoStringDictionary = true;
            ServiceStack.Text.JsConfig.IncludeNullValues = false;
        }


        public Dictionary<String, String> AsDictionary<T>(T objectToConvert) where T : CommonParameters
        {
            return objectToConvert == null ?
                null :
                ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<String, String>>(ServiceStack.Text.JsonSerializer.SerializeToString(objectToConvert));
        }


        public T AsPortalResponse<T>(String dataToConvert) where T : PortalResponse
        {
            return String.IsNullOrWhiteSpace(dataToConvert)
                ? null
                : ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(dataToConvert);
        }
    }

}
