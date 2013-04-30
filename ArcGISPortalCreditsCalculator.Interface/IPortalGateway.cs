using ArcGISPortalCreditsCalculator.Interface.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISPortalCreditsCalculator.Interface
{
    public interface IPortalGateway
    {
        String RootUrl { get; set; }

        Token Token { get; set; }

        Task<List<Item>> HydrateItems(ICollection<String> itemIds);
    }
}
