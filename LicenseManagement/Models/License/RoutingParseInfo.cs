using System.Collections.Generic;

namespace LicenseManagement.Models.License
{
    public class RoutingParseInfo
    {
        public string FieldName { get; set; }
        public int OriginalOrder { get; set; }
        public int CustomOrder { get; set; }
        public List<int> FieldValue { get; set; }
        public bool DisableField { get; set; }
    }
}