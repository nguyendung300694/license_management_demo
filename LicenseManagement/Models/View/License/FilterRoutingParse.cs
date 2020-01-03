namespace LicenseManagement.Models.View.License
{
    public class FilterRoutingParse
    {
        public int FieldCount { get; set; }
        public string RoutingId { get; set; }
        public int ProductInd { get; set; }
        public int FormatInd { get; set; }

        public string ProductName { get; set; }
        public string FormatName { get; set; }
        public string[] FieldFilter { get; set; }

        /// <summary>
        /// LicenseType
        /// 1: Hardware License
        /// 2: Software License
        /// </summary>
        public int LicenseType { get; set; }
        //public ArrayList FieldFilter { get; set; }

        ////for cms server only
        //public string DVRCount { get; set; }

        ////for spxpro   
        //public string Pacdm { get; set; }
        //public string Fps { get; set; }
        //public string Audio { get; set; }
        //public string IPCamera { get; set; }
        //public string VLBasic { get; set; }
        //public string VCBasic { get; set; }

        //public string VLPlus { get; set; }
        //public string VCPlus { get; set; }
        //public string LPR { get; set; }
        //public string CMSMode { get; set; }
    }
}