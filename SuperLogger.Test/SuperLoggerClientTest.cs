using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using SuperLogger.Client;
using SuperLogger.Model;

namespace SuperLogger.Test
{
    [TestClass]
    public class SuperLoggerClientTest
    {
        [TestMethod]
        public void InfoTest()
        {
            SuperLoggerClient.Info("Info message");
        }

        [TestMethod]
        public void AddLogEntryTest()
        {
            SuperLoggerDbModel dbModel = new SuperLoggerDbModel();
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new KeyValuePair<string, string>("Name.NOSOURCE." + i.ToString(), "Value." + i.ToString()));
            }
            dbModel.AddLogEntry("Message 1", SuperLoggerDbModel.LogType.INFO, data);

        }
    }
}