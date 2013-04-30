using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISPortalCreditsCalculator.Interface
{
    public interface IRuleEngine
    {
        double CalculateCredits(IRuleCollection rules);
    }
}
