using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseManagement.App_Start
{
    public class LoggerConfig
    {
        public static void Initialize()
        {
            var logFile = HttpContext.Current.Server.MapPath("~/I3App_Files/Logs/Log_{Date}.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RollingFile(logFile)
                .CreateLogger();
        }
    }
}