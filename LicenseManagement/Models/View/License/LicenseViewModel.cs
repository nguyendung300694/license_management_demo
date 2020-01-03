namespace LicenseManagement.Models.View.License
{
    public class LicenseViewModel
    {
        public int CompanyID { get; set; }
        public string ScreenID { get; set; }

        public string OrderType { get; set; }
        public string SalesOrderNbr { get; set; }
        public string CustomerPONbr { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNbr { get; set; }
        public string DocType { get; set; }
        public string WorkOrderNbr { get; set; }

        public int Type { get; set; }
        public string SerialNbr { get; set; }

        public string MachineCode { get; set; }
        public string UniqueId { get; set; }      
        public string RoutingNbr { get; set; }
        public bool AllowMultiMAC { get; set; }
        public SaveLicenseSetting SaveLicenseSetting { get; set; }
    }
}