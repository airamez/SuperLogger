using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

using RabbitMQ.Client;
using Newtonsoft.Json;

namespace SuperLogger.Client
{
    public static class SuperLoggerClient
    {

        private static string RMQ_SETTINGS_KEY_NAME = "RMQ.Settings";
        private static RmqSettings _rmqSettings;

        static SuperLoggerClient()
        {
            try
            {
                _rmqSettings = JsonConvert.DeserializeObject<RmqSettings>(GetAppSettings(RMQ_SETTINGS_KEY_NAME));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Invalid {0} key, Error = {1}",
                                    RMQ_SETTINGS_KEY_NAME, ex.Message));
            }
        }

        private static IConnection OpenRmqConnection()
        {
            var _rmqFactory = new ConnectionFactory();
            _rmqFactory.HostName = _rmqSettings.HostName;
            _rmqFactory.VirtualHost = _rmqSettings.VirtualHost;
            _rmqFactory.UserName = _rmqSettings.UserName;
            _rmqFactory.Password = _rmqSettings.Password;

            var _rmqConn = _rmqFactory.CreateConnection();
            return _rmqConn;
        }

        private static IModel OpenRmqModel(IConnection rmqConn)
        {
            var _rmqModel = rmqConn.CreateModel();
            return _rmqModel;
        }

        private static string GetAppSettings(string keyName)
        {
            string value = ConfigurationManager.AppSettings[keyName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(string.Format("Key {0} not found on configuration file.", RMQ_SETTINGS_KEY_NAME));
            }
            return value;
        }

        // @todo: Add a generic dictionary parameter to allow list of names and values
        public static void Info (string message)
        {
            SendMessage(message);
        }

        private static void SendMessage(string message)
        {
            IConnection rmqConn = null;
            IModel rmqModel = null;
            try
            {
                rmqConn = OpenRmqConnection();
                rmqModel = OpenRmqModel(rmqConn);
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message);
                rmqModel.BasicPublish(_rmqSettings.Exchange, _rmqSettings.RoutingKey, null, messageBodyBytes);
            } catch (Exception ex)
            {
                string source = "Supper Logger";
                string log = "Application";
                string eventMessage = string.Format("Error: {0} \nStack Trace: {1}", ex.Message, ex.StackTrace);

                // This requires admin privilege
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, log);
                EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Error);
            }
            finally
            {
                CloseRmqConnection(rmqConn);
            }
        }

        private static void CloseRmqConnection(IConnection rmqConn)
        {
            if (rmqConn != null && rmqConn.IsOpen)
            {
                rmqConn.Close();
            }
        }

        // @todo: Add a generic dictionary parameter to allow list of names and values
        public static void Error(string message)
        {

        }
    }
}
