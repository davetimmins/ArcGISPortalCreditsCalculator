using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISPortalCreditsCalculator.Interface
{
    public interface IRuleCollection
    {
        IEnumerable<IRule> Rules { get; set; }
    }
}
