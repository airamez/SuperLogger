using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLogger.Helper
{
    public static class SuperLoggerHelper
    {
        public static string GetAppSettings(string keyName)
        {
            string value = ConfigurationManager.AppSettings[keyName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(string.Format("Key {0} not found on configuration file.", keyName));
            }
            return value;
        }

    }
}
