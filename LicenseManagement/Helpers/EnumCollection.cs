using System.ComponentModel;
using System.Runtime.Serialization;

namespace LicenseManagement.Helpers
{
    public class EnumCollection
    {
        //public enum DefinedSpecialView
        //{
        //    ViewCost = 1,
        //    EditCost = 2
        //}

        //public enum DefinedSpecialMenuFunction
        //{
        //    ViewCost = 5,
        //    SetupItem = 6
        //}

        //public enum ErrorTransferInType
        //{
        //    ReadyForTransfer = 0,
        //    DuplicateItem = 1,
        //    ExistingInDb = 2,
        //    NotScanBefore = 3
        //}

        //public enum CheckingSerialType
        //{
        //    ById = 1,
        //    BySerial = 2
        //}

        //public enum CustomAttributeMode
        //{
        //    Enforce,
        //    Ignore
        //}

        //public enum CommentLogPhaseType
        //{
        //    ErrorLog = 1,
        //    CausedBy = 2
        //}

        //public enum ErrorType
        //{
        //    ProductionErrorType = 1
        //}

        //public enum AllProgram
        //{
        //    DSSLite = 1,
        //    P3 = 2,
        //    SPK = 3
        //}

        //public enum CompanyTenantID
        //{
        //    I3I = AppSettings.CompanyID_CAD,
        //    I3N = AppSettings.CompanyID_USD,
        //    JK = AppSettings.CompanyID_JK,
        //    I3I_TEST = AppSettings.CompanyID_Test_CAD,
        //    I3N_TEST = AppSettings.CompanyID_Test_USD,
        //    JK_TEST = AppSettings.CompanyID_Test_JK
        //}

        public enum UOMBoxOrItem
        {
            MM = 1,
            Inches = 2,
            Kg = 3,
            Pound = 4,
            CM = 5
        }

        //public enum LicenseStatus
        //{
        //    Available = 1,
        //    Used = 2
        //}

        public enum LicenseType
        {
            WorkOrder = 1,
            LicenseSoftware = 2
        }

        //public enum SettingKeyType
        //{
        //    LicenseSetting = 1,
        //}

        //public enum KPIDVRType
        //{
        //    DVR = 1,
        //    VEO = 2,
        //    Encoder = 3
        //}

        public enum CommentAction
        {
            Insert = 0,
            Update = 1,
            Delete = 2
        }

        //public enum SOTYpe
        //{
        //    SOForStockWO = -999,
        //    SOForUpgradegeInvoice = -998
        //}

        //public enum QRCodeType
        //{
        //    Location = 1,
        //    Box = 2,
        //    Item = 3
        //}

        //public enum SessionKey
        //{
        //    UserLogin,                 
        //    UserEmail,
        //    LoginEmailInstance
        //}

        //public enum CookiesKey
        //{
        //    UserLogin,
        //    EmailPassword
        //}

        //public enum InvoiceEmailStatus
        //{
        //    IsSent = 0,
        //    IsApproved = 1,
        //    IsResent = 2
        //}

        //public enum MailLogType
        //{
        //    Statement = 1, // key will by CustKey
        //    AcknowledgeSO = 2, // key will by SOKey
        //    LicenseKey = 3, // key will by WorkOrderKey
        //}

        //public enum ScanningStatus
        //{
        //    IsScanned = 0, // just scanner first time
        //    IsApproved = 1, // approve by second person
        //    IsMarried = 2, // married box and rack
        //    IsDiscrepancy = 3, // second person said not enough
        //    IsPallet = 4, // qr code for pallet
        //}

        //public enum InventoryLogTransactionType
        //{
        //    IN = 0,
        //    OUT = 1,
        //    FLY = 2
        //}

        /*
        public enum DemoInvoice
        {
            [EnumMember(Value = "Not Set")]
            [Description("Not Set")]
            Zero = 0,

            [EnumMember(Value = "60 days")]
            [Description("60 days")]
            SixtyDays = 60,

            [EnumMember(Value = "90 days")]
            [Description("90 days")]
            NinetyDays = 90,

            [EnumMember(Value = "120 days")]
            [Description("120 days")]
            TwelveDays = 120,

            [EnumMember(Value = "Other")]
            [Description("Other")]
            Other = 999,
        }
        */

        //public enum DefectiveType
        //{
        //    Component = 1,
        //    Defect = 2,
        //    Solution = 3,
        //}

        //public enum Actions
        //{
        //    Insert = 0,
        //    Update = 1,
        //    Delete = 2
        //}

        //public enum DefectiveItem
        //{
        //    DVR = 0,
        //    Component = 1,
        //    Single = 2
        //}

        //public enum LDAPFind
        //{
        //    UserId = 0,
        //    Email = 1
        //}

        //public enum WOPL
        //{
        //    WO = 1,
        //    PL = 2,
        //    SO = 3,
        //    IN = 4,
        //    MthIN = 5,
        //    LabourEntry = 6
        //}

        //public enum AbNormalProcessType
        //{
        //    CloseNonNormal = 1,
        //    ReverseWO = 2
        //}

        //public enum AttachedFileTypeKey
        //{
        //    SO = 1,
        //    WO = 2, 
        //    PL = 3,
        //    SystemCheckList = 4,
        //    QASystemCheckList = 5,
        //    ShippingCheckList = 6,
        //    IN = 7,
        //    EmailMsg = 8, 
        //    QAShippingCheckList = 9,
        //    PhaseACheckList = 10
        //}

        //public enum POStatus
        //{
        //    Open = 1,
        //    Canceled = 3,
        //    Closed = 4
        //}

        /*
        public enum ProductionStatus
        {
            [EnumMember(Value = "Unacknowledged")]
            [Description("Unacknowledged")]
            Unacknowledged = 0,

            [EnumMember(Value = "Open")]
            [Description("Open")]
            Open = 1,

            [EnumMember(Value = "Canceled")]
            [Description("Canceled")]
            Canceled = 3,

            [EnumMember(Value = "Closed")]
            [Description("Closed")]
            Closed = 4,

            [EnumMember(Value = "Incompleted")]
            [Description("Incompleted")]
            Incompleted = 5,

            [EnumMember(Value = "Ready for Inventory")]
            [Description("Ready for Inventory")]
            ReadyForInventory = 99,

            [EnumMember(Value = "Ready for Phase A")]
            [Description("Ready for Phase A")]
            ReadyForPhaseA = 100,

            [EnumMember(Value = "Ready for Phase B")]
            [Description("Ready for Phase B")]
            ReadyForPhaseB = 101,

            [EnumMember(Value = "Ready for System QA")]
            [Description("Ready for System QA")]
            ReadyForQA1 = 102,

            [EnumMember(Value = "Ready for Shipping")]
            [Description("Ready for Shipping")]
            ReadyForShipping = 103,

            [EnumMember(Value = "Ready for Shipping QA")]
            [Description("Ready for Shipping QA")]
            ReadyForQA2 = 104,

            [EnumMember(Value = "Ready for Create PL")]
            [Description("Ready for Create PL")]
            ReadyForCreatePL = 105,

            [EnumMember(Value = "Ready for Accounting")]
            [Description("Ready for Accounting")]
            ReadyForAccounting = 106,
            
            [EnumMember(Value = "Cancel Abnormal")]
            [Description("Cancel Abnormal")]
            CancelAbnormal = 998,

            [EnumMember(Value = "All")]
            [Description("All")]
            All = 999,
        }
        */

        //public enum InventoryTabIndex
        //{
        //    WOTab = 0,
        //    PLTab = 1,
        //    SOTab = 2
        //}

        //public enum ShippingTabIndex
        //{
        //    WOTab = 0,
        //    PLTab = 1
        //}

        //public enum NCRClosedStatus
        //{
        //    Open,
        //    Closed,
        //    Draft
        //}

        //public enum NCRUser
        //{
        //    HeadDept = 41,
        //    Normal = 43,
        //    NCRAdmin = 44
        //}

        //public enum NCRIssue
        //{

        //}

        public enum QRecLevel
        {
            QR_ECLEVEL_L = 0, //< lowest
            QR_ECLEVEL_M,
            QR_ECLEVEL_Q,
            QR_ECLEVEL_H      //< highest
        }

        public struct QRcode
        {
            public int version;
            public int width;
            public bool[] data;
        }

        //public enum SPKLogOperation
        //{
        //    Generate = 1,
        //    Update = 2,
        //    Other = 3
        //}

        public enum TypeExpired
        {
            None = 1,
            ExpiredNumber = 2,
            ExpiredDate = 3
        }

        //public enum AutoSerialKeyType
        //{
        //    WOKey = 1,
        //    SOLineKey = 2
        //}

        //public enum CommentPhase
        //{
        //    FullPharse = 0,
        //    PhaseA = 1,
        //    PhaseB = 2,
        //    QASystem = 3,
        //    QAShipping = 4
        //}

        //public enum ItemStatus
        //{
        //    Active = 1,
        //    Inactive = 2,
        //    Discontinued = 3,
        //    Deleted = 4,
        //    RechangeQRCode = 998,
        //    Defective = 999
        //}

        //public enum ItemType
        //{
        //    Serialize = 0,
        //    NonSerialize = 1
        //}

        //public enum DateOffRemark
        //{
        //    OffWholeDay = 1,
        //    OffHalfDay = 2,
        //    Holiday = 3,
        //    Weekend = 4
        //}

        public enum UpgradeLicenseType
        {
            Analog = 1,
            IPP = 2,
            PACDM = 3,
            VL = 4,
            VLP = 5,
            ExternalMonitor = 6
        }

        //public enum HistoryInventoryLog 
        //{
        //    TransferIn = 0,
        //    TransferOut = 1,
        //    UpdateNonSerQty = 997,
        //    ReprintQRCode = 998
        //}

        //public enum GetUserType
        //{
        //    Login = 0,
        //    ViewInfo = 1
        //}

        //public enum DefinedRmaFunction
        //{
        //    TechSupport = 7,
        //    Care = 8,
        //    RmaTech = 9
        //}
    }
}