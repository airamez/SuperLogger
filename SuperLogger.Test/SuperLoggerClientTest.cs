using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperLogger.Client;

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
        public void DBTest()
        {

        }
    }
}
