using Newtonsoft.Json;
using RabbitMQ.Client;
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
        public static string RMQ_SETTINGS_KEY_NAME = "SuperLogger.RMQ.Settings";
        public static string SQL_CONNECTION_STRING = "SuperLogger.SQL.ConnectionString";

        private static RmqSettings _rmqSettings = ReadRmqSettings();

        public static RmqSettings RmqSettings
        {
            get
            {
                return _rmqSettings;
            }
        }

        public static string Exchange
        {
            get
            {
                return _rmqSettings.Exchange;
            }
        }

        public static string RoutingKey
        {
            get
            {
                return _rmqSettings.RoutingKey;
            }
        }

        public static string QueueName
        {
            get
            {
                return _rmqSettings.Queue;
            }
        }

        public static string GetAppSettings(string keyName)
        {
            string value = ConfigurationManager.AppSettings[keyName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(string.Format("Key {0} not found on configuration file.", keyName));
            }
            return value;
        }

        private static RmqSettings ReadRmqSettings()
        {
            try
            {
               _rmqSettings = JsonConvert.DeserializeObject<RmqSettings>(SuperLoggerHelper.GetAppSettings(SuperLoggerHelper.RMQ_SETTINGS_KEY_NAME));
                return _rmqSettings;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Invalid {0} key, Error = {1}",
                                    SuperLoggerHelper.RMQ_SETTINGS_KEY_NAME, ex.Message));
            }
        }

        public static IConnection GetRmqConnection()
        {
            var _rmqFactory = new ConnectionFactory();
            _rmqFactory.HostName = _rmqSettings.HostName;
            _rmqFactory.Port = _rmqSettings.Port;
            _rmqFactory.VirtualHost = _rmqSettings.VirtualHost;
            _rmqFactory.UserName = _rmqSettings.UserName;
            _rmqFactory.Password = _rmqSettings.Password;

            var _rmqConn = _rmqFactory.CreateConnection();
            return _rmqConn;
        }
    }
}
