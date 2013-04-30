using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISPortalCreditsCalculator.Interface
{
    /// <summary>
    /// Represents a simple rule
    /// </summary>
    public interface IRule
    {
        int NumberOfOperations { get; set; }

        double CreditsPerOperation { get; }

        /// <summary>
        /// Used to map the rule to an <see cref="Item"/> type
        /// </summary>
        String Type { get; }
    }
}
