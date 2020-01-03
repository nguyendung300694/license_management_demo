using System.Collections.Generic;

namespace LicenseManagement.Models.View.License
{
    public class SelectValue
    {
        public int Id { get; set; }
        public string DropDownValue { get; set; }
        public int OriginalOrder { get; set; }
        public int CustomOrder { get; set; }
        public int CustomColumn { get; set; }
        public bool VisibleField { get; set; }
        public int CustomStandardNonStandard { get; set; } // 1 : standard ; 2 non-standard
        public List<SelectValue> ChildSelectValued { get; set; }
    }

    public class I3Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}