using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

using RabbitMQ.Client;
using Newtonsoft.Json;

using SuperLogger.Helper;

namespace SuperLogger.Client
{
    public static class SuperLoggerClient
    {

        private static string _correlationID;

        static SuperLoggerClient()
        {
        }

        public static string CorrelationID
        {
            set
            {
                _correlationID = value;
            }

            get
            {
                return _correlationID;
            }
        }

        public static void Info(string source, string message, IDictionary<string, string> data = null, bool? throwExceptions = null)
        {
            SendMessage(SuperLoggerHelper.GetLogEntryTypeText(LogType.INFO), source, message, null, data);
        }

        public static void Error(string source, string message, string stackTrace, IDictionary<string, string> data = null)
        {

        }

        private static void SendMessage(string type, 
                                        string source, 
                                        string message, 
                                        string stackTrace, 
                                        IDictionary<string, string> data = null, 
                                        bool? throwExceptions = null)
        {
            IConnection rmqConn = null;
            IModel rmqModel = null;
            try
            {
                rmqConn = SuperLoggerHelper.GetRmqConnection();
                rmqModel = rmqConn.CreateModel();
                // @This is unecessary and may impact performance. It will require pre rabbit mq configuration to create the Ex
                // Better to move this to a initialization area
                //rmqModel.ExchangeDeclare(SuperLoggerHelper.Exchange, ExchangeType.Direct);
                //rmqModel.QueueDeclare(SuperLoggerHelper.QueueName, true, false, false, null);
                //rmqModel.QueueBind(SuperLoggerHelper.QueueName, SuperLoggerHelper.Exchange, SuperLoggerHelper.RoutingKey, null);
                RmqMessageContent messageContent = PrepareMessageContent(type, source, message, stackTrace, data);
                string messageBody = JsonConvert.SerializeObject(messageContent);
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(messageBody);
                rmqModel.BasicPublish(SuperLoggerHelper.Exchange, SuperLoggerHelper.RoutingKey, null, messageBodyBytes);
            }
            catch (Exception ex)
            {
                if (throwExceptions.HasValue && throwExceptions.Value)
                {
                    throw;
                }

                // @todo: Add the exception to Event Viewer

                //string source = "Super Logger";
                //string log = "Application";
                //string eventMessage = string.Format("Error: {0} \nStack Trace: {1}", ex.Message, ex.StackTrace);

                //// @todo: This requires admin privilege. Check how to make it always work
                //if (!EventLog.SourceExists(source))
                //    EventLog.CreateEventSource(source, log);
                //EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Error);
            }
            finally
            {
                CloseRmqConnection(rmqConn);
            }
        }

        private static RmqMessageContent PrepareMessageContent(string type, string source, string message, string stackTrace, IDictionary<string, string> data)
        {
            RmqMessageContent messageContent = new RmqMessageContent();
            messageContent.LogType = type;
            messageContent.Source = source;
            messageContent.Message = message;
            messageContent.StackTrace = stackTrace;
            messageContent.Data = data;
            messageContent.CreatedOn = JsonConvert.SerializeObject(DateTime.Now.ToUniversalTime());
            messageContent.CorrelationID = CorrelationID;
            return messageContent;
        }

        private static void CloseRmqConnection(IConnection rmqConn)
        {
            try
            {
                if (rmqConn != null && rmqConn.IsOpen)
                {
                    rmqConn.Close();
                }
            }
            catch { }
        }
    }
}
