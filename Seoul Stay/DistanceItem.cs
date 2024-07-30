using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seoul_Stay
{
    public class DistanceItem
    {
        public int AttractionID { get; set; }
        public string AttractionName { get; set; }
        public string AreaName { get; set; }
        public decimal Distance { get; set; }
        public int OnFoot { get; set; }
        public int ByCar { get; set; }
    }
}
