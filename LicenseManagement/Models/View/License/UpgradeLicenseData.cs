using LicenseManagement.Helpers;

namespace LicenseManagement.Models.View.License
{
    public class UniqueSerialNumberDetail
    {
        public int CompanyID { get; set; }

        public string DocType { get; set; }
        public string RefNbr { get; set; }
        public int RefLineNbr { get; set; }

        public string ParentDocType { get; set; }
        public string ParentRefNbr { get; set; }

        public string CustomerPONbr { get; set; }
        public string SerialNbr { get; set; }
    }

    public class UpgradeLicenseData
    {
        public int CompanyID { get; set; }
        public string ScreenID { get; set; }

        public int RefLineNbr { get; set; }        
        public int Type { get; set; }
        public string SerialNbr { get; set; }
        public int ProductId { get; set; }
        public int UpdateQty { get; set; }

        public string OrderType { get; set; }
        public string SalesOrderNbr { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNbr { get; set; }
        public string CustomerPONbr { get; set; }
        public string RoutingNbr { get; set; }
        
        public EnumCollection.UpgradeLicenseType UpgradeLicenseType { get; set; }
    }
}