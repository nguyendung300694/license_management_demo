//using PX.Data;
using System;

namespace LicenseManagement
{
    public static class AppSettings
    {
        public const string SecurityKey = "Thai Quoc Dung";

        public const string TrackingApiKey = "FaNr0hOpLvSrh7kZGuL2sA";

        public const string LDAPServer = "192.168.10.2";
        public const string LDAPPort = "389";
        public const string LDAPPath = "LDAP://192.168.10.2/OU=i3users,DC=i3international,DC=local";
        public const string LDAPUser = "CN=General Account,OU=GenAccounts,OU=i3Users,DC=i3international,DC=local";
        public const string LDAPPassword = "i3G3n@cc";

        public const string FindBoxApi = "http://www.packit4me.com/api/call/raw";
        public const string i3packingapi_auth_username = "weblicense";
        public const string i3packingapi_auth_password = "2R9ytaBrsQaTAyYJmCagnA==";
        public const string i3packingapi = "https://erp-api.i3international.com";
        public const string packing_bin_max_weight = "30";
        public const string _3dbp_api_username = "i3international";
        public const string _3dbp_api_key = "97197336264466beab839182a62193d0";
        public const string _3dbp_api_packtoonebox = "https://us-east.api.3dbinpacking.com/packer/pack";
        public const string _3dbp_api_packtomanybox = "https://us-east.api.3dbinpacking.com/packer/packIntoMany";

        public const string Company_Addr_Line_1 = "780 Birchmount Rd";
        public const string Company_Addr_Line_2 = "Unit 16";
        public const string Company_Addr_Line_3 = "Richmond Hill";
        public const string ShipperCity = "SCARBOROUGH";
        public const string ShipperPostalCode = "M1K5H4";
        public const string ShipperStateProvinceCode = "ON";
        public const string ShipperCountryCode = "CA";
        public const string ShipFromCity = "Toronto";
        public const string ShipFromPostalCode = "M1K5H4";
        public const string ShipFromStateProvinceCode = "ON";
        public const string ShipFromCountryCode = "CA";

        public const string ar_account_mail = "i3ar@i3international.com";
        public const string ap_account_mail = "i3ap@i3international.com";
        public const string orders_account_mail = "orders@i3international.com";
        public const string mail_username = "genacc";
        public const string mail_pass = "i3G3n@cc";
        public const string orders_mailbox_username = "hiep@i3international.com";
        public const string orders_mailbox_pass = "passw0rd@i1";
        public const string EmailServer = "https://exchange.i3international.com/ews/exchange.asmx";
        public const string EmailServer_Office365 = "https://outlook.office365.com/ews/exchange.asmx";

        public const string SOType_IC_CAD = "IC";
        public const string SOType_RR = "RR";
        public const string SOType_IN = "IN";
        public const string AdvReplaceOrderType = "RA";
        public const string SOType_SP = "SP";
        public const string SOType_DP = "DP";
        public const int CompanyID_CAD = 2; //i3 International Inc. = 2
        public const int CompanyID_JK = 3;// JK Quality = 3
        public const int CompanyID_USD = 4;// i3 America Nevada Inc. = 4
        public const int CompanyID_Test_CAD = 6;//Test CAD = 6
        public const int CompanyID_Test_USD = 7;// Test USD = 7
        public const int CompanyID_Test_JK = 9;// Test JK = 9

        public const string InvoiceNbrAutoReceipt = "Auto receipt when {0} order shipped out";
        public const string UPSSTANDARD_ShipVia = "UPSSTANDARD";
        public const string Customer_US_CD = "C0000067";//i3 international tenant
        public const string Customer_JK_CD = "C0000081";//i3 international tenant
        public const string VendorAcctCD_I3Inc_US = "V0000641";// add vendor for Stock, Non-stock items  in US 
        public const string VendorAcctCD_I3Inc_JK = "V0000002";
        public const string BranchCD_CA = "MAIN";
        public const string ShipmentScreenID = "SO.30.20.00";
        public const string POReceiptScreenID = "PO.30.20.00";
        public static DateTime ICSolutionReleaseDate = new DateTime(2019, 3, 1);
        public static DateTime JKStepSolutionReleaseDate = new DateTime(2019, 4, 1);
        public static DateTime PriceBetweenI3NAndI3ISolution = new DateTime(2019, 7, 1);
        public const string AdminLoginName = "admin";
        public static decimal? DefaultQty = 1m;
        public const string FreightIn_ItemCD = "FREIGHT-IN";
        public static decimal? XXXRate = 0.7m;

        //public const string ScreenIDImportScenario = "SM.20.60.36";
        //public const string ScreenIDImportScenarioProcess = "SM.20.60.35";
        //public const string Customer_US_Name = "i3 America Nevada Inc.";
        //public const string Customer_JK_Name = "i3 JK Quality Canada";

        //testing
        //public const int CompanyID_CAD = 6; //
        //public const int CompanyID_USD = 7;// 
        //public const int CompanyID_JK = 9;//
        //public const int CompanyID_Test_CAD = 2;//
        //public const int CompanyID_Test_USD = 4;// 
        //public const int CompanyID_Test_JK = 3;//
        //./testing
        //public class soType_IC_CAD : Constant<string> { public soType_IC_CAD() : base(SOType_IC_CAD) { } }

        public const string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;Mode=Read;Extended Properties=\'Excel 8.0;HDR={1};IMEX=1\'";
        public const string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;Mode=Read;Extended Properties=\'Excel 8.0;HDR={1};IMEX=1\'";
    }

    ///
    /// READ SETTINGS FROM app.config FILE
    /// 
    /*     
    public static class AppSettings
    {
        public static string GetValue(string key)
        {
            Configuration config = null;
            string exeConfigPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                //Open the configuration file using the dll location
                config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);

                if (config != null)
                {
                    KeyValueConfigurationElement element = config.AppSettings.Settings[key];
                    if (element != null)
                    {
                        string value = element.Value;
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    */
}
