﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using SuperLogger.Client;
using SuperLogger.Model;
using SuperLogger.Helper;

namespace SuperLogger.Test
{
    [TestClass]
    public class SuperLoggerClientTest
    {
        //@todo: Create real unit test. Those are not unit test :P

        [TestMethod]
        public void LogInfoTest()
        {
            SuperLoggerClient.CorrelationID = Guid.NewGuid().ToString();

            IDictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new KeyValuePair<string, string>("Name.NOSOURCE." + i.ToString(), "Value." + i.ToString()));
            }

            SuperLoggerClient.Info("MySource", "Info message: " + DateTime.Now, data);
            SuperLoggerClient.Info("MySource", "Info message: " + DateTime.Now);
            SuperLoggerClient.Info("MySource", "Info message: " + DateTime.Now);
            SuperLoggerClient.Info("MySource", "Info message: " + DateTime.Now);
            for (int i = 0; i < 10; i++ )
            {
                SuperLoggerClient.Info("MySource", "Info message: " + DateTime.Now);
                SuperLoggerClient.CorrelationID = Guid.NewGuid().ToString();
            }

            SuperLoggerClient.CorrelationID = null;
            SuperLoggerClient.Info("MySource", "Info message: " + DateTime.Now);

            SuperLoggerClient.Info("MySource", "Loggin Message", null, true);
        }

        [TestMethod]
        public void AddLogEntryTest()
        {
            SuperLoggerDbModel dbModel = new SuperLoggerDbModel();
            IDictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new KeyValuePair<string, string>("Name.NOSOURCE." + i.ToString(), "Value." + i.ToString()));
            }
            dbModel.AddLogEntry("Test", "I", Guid.NewGuid().ToString(), DateTime.Now, "Message 1", "StackTrace", data);
        }

        [TestMethod]
        public void ServiceModelTest()
        {
            SuperLoggerServiceModel service = new SuperLoggerServiceModel();
            service.Run();
        }
    }
}