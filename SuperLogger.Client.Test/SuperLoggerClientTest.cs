using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SuperLogger.Client.Test
{
    [TestClass]
    public class SuperLoggerClientTest
    {
        [TestMethod]
        public void InfoTest()
        {
            SuperLoggerClient.Info("Info message");
        }
    }
}
