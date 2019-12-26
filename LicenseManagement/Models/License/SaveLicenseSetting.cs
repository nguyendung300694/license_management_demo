using System;
using System.Collections;
using LicenseManagement.Helpers;

namespace LicenseManagement.Models.License
{
    public class SaveLicenseSetting
    {
        public string MachineCode { get; set; }
        public string UniqueId { get; set; }
        public int ProductId { get; set; }
        public int FormatId { get; set; }
        public EnumCollection.TypeExpired TypeExpired { get; set; }
        public int? ExpiredDay { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public ArrayList SettingDetails { get; set; }
    }

    //This class only use to get license setting with SN extension (do not use for save license setting)
    public class SaveLicenseSettingExt : SaveLicenseSetting 
    {
        public string Extension { get; set; }
        public string Model { get; set; }
        public bool HASPReplaced { get; set; }
        public bool AllowMultiMAC { get; set; }
    }
}