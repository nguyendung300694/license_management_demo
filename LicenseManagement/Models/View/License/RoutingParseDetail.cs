using System.Collections.Generic;

namespace LicenseManagement.Models.View.License
{
    public class RoutingParseDetail
    {
        public string RoutingParseType { get; set; }
        public int RoutingType { get; set; } // 1 : SPXProHighLevel 2 : SPXProLowLevel 3 : CMSServer

        //public List<License_t> RoutingParseDetail { get; set; }
        public List<RoutingParseInfo> RoutingParseInfo { get; set; }
        public bool NeedToFilter { get; set; } // need to filter beacause it's so many parse info

        //public List<License_t> SPXProHighs { get; set; }
        //public List<License_t> SPXProLows { get; set; }
        //public List<License_t> CMSServers { get; set; }
    }
}