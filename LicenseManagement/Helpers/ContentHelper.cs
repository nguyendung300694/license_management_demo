namespace LicenseManagement.Helpers
{
    public static class FileAutoVersioning
    {
        private static readonly string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string CreateVersionName(string fileName)
        {
            return string.Format("{0}?v={1}", fileName, Version);
        }
    }

    public static class ContentHelper
    {
        public static string LoadContent(string fileName)
        {
            return string.Format("../../../Content/{0}", FileAutoVersioning.CreateVersionName(fileName));
        }
        
        public static string LoadScript(string fileName)
        {
            return string.Format("../../../Scripts/{0}", FileAutoVersioning.CreateVersionName(fileName));
        }
    }
}
