using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISPortalCreditsCalculator.Interface.Types
{
    public abstract class PortalObject
    {
        public PortalError Error { get; set; }
    }

    public class PortalError
    {
        public String Code { get; set; }        
        public String Message { get; set; }

        public String ExceptionMessage { get { return String.Format("{0} - {1}", Code, Message); } }
    }
}
