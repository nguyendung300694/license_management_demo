using Serilog;
using System;
using System.Web;

namespace LicenseManagement.Services.Logger
{
    public class Logger : ILogger
    {
        public void Info(string methodName, string message)
        {
            try
            {
                Log.Information(string.Format("{0} - {1} : {2}", HttpContext.Current.User.Identity.Name, methodName, message));
            }
            catch (Exception ex)
            {
                Log.Error("Error - Cannot log Information: " + ex.Message);
            }
        }

        public void Info(string methodName, string message, params object[] p)
        {
            try
            {
                Log.Information(string.Format("{0} - {1} : {2}", HttpContext.Current.User.Identity.Name, methodName, message), p);
            }
            catch (Exception ex)
            {
                Log.Error("Error - Cannot log Information ext: " + ex.Message);
            }
        }

        public void Error(string methodName, string message)
        {
            try
            {
                Log.Error(string.Format("{0} - {1} : {2}", HttpContext.Current.User.Identity.Name, methodName, message));
            }
            catch (Exception ex)
            {
                Log.Error("Error - Cannot log Error: " + ex.Message);
            }
        }

        public void Error(string methodName, string message, params object[] p)
        {
            try
            {
                Log.Error(string.Format("{0} - {1} : {2}", HttpContext.Current.User.Identity.Name, methodName, message), p);
            }
            catch (Exception ex)
            {
                Log.Error("Error - Cannot log Error ext: " + ex.Message);
            }
        }

        public void Fatal(string methodName, string message)
        {
            try
            {
                Log.Fatal(string.Format("{0} - {1} : {2}", HttpContext.Current.User.Identity.Name, methodName, message));
            }
            catch (Exception ex)
            {
                Log.Error("Error - Cannot log Fatal: " + ex.Message);
            }
        }

        public void Fatal(string methodName, string message, params object[] p)
        {
            try
            {
                Log.Fatal(string.Format("{0} - {1} : {2}", HttpContext.Current.User.Identity.Name, methodName, message), p);
            }
            catch (Exception ex)
            {
                Log.Error("Error - Cannot log Fatal ext: " + ex.Message);
            }
        }
    }
}