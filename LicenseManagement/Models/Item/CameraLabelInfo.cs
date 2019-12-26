namespace LicenseManagement.Models.Item
{
	public class CameraLabelInfo
	{
        public string ItemID { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string Description { get; set; }
        public string UPCCode { get; set; }
        public string MACAddress { get; set; }
        public bool HasCELogo { get; set; }
    }
}