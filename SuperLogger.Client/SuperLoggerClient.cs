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
    /// <summary>
    /// SuperLogger main class provinding static methods for logging
    /// </summary>
    public static class SuperLoggerClient
    {

        private static string _correlationID;

        private const string INFO = "I";
        private const string WARN = "W";
        private const string ERROR = "E";

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

        /// <summary>
        /// Logs as INFO
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        public static void Info(string source, string message)
        {
            SendMessage(INFO, source, message, null, null, false);
        }

        /// <summary>
        /// Logs as INFO
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        public static void Info(string source, string message, IDictionary<string, string> data)
        {
            SendMessage(INFO, source, message, null, data, false);
        }

        /// <summary>
        /// Logs as INFO
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Info(string source, string message, bool throwExceptions)
        {
            SendMessage(INFO, source, message, null, null, throwExceptions);
        }

        /// <summary>
        /// Logs as INFO
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Info(string source, string message, IDictionary<string, string> data, bool throwExceptions)
        {
            SendMessage(INFO, source, message, null, data, throwExceptions);
        }

        /// <summary>
        /// Log as Warning
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        public static void Warn(string source, string message)
        {
            SendMessage(WARN, source, message, null, null, false);
        }

        /// <summary>
        /// Log as Warning
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        public static void Warn(string source, string message,
                                IDictionary<string, string> data)
        {
            SendMessage(WARN, source, message, null, data, false);
        }

        /// <summary>
        /// Log as Warning
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Warn(string source, string message,
                                bool throwExceptions)
        {
            SendMessage(WARN, source, message, null, null, false);
        }

        /// <summary>
        /// Log as Warning
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Warn(string source, string message,
                                IDictionary<string, string> data,
                                bool throwExceptions)
        {
            SendMessage(WARN, source, message, null, data, throwExceptions);
        }

        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        public static void Error(string source, string message)
        {
            SendMessage(ERROR, source, message, null, null, false);
        }

        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="Exception">Exception. The Exception.Message and the Exception.StackTrace will be used automatically</param>
        public static void Error(string source, Exception exception)
        {
            SendMessage(ERROR, source, exception.Message, exception.StackTrace, null, false);
        }

        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        public static void Error(string source,
                                 Exception exception,
                                 IDictionary<string, string> data)
        {
            SendMessage(ERROR, source, exception.Message, exception.StackTrace, data, false);
        }


        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="Exception">Exception. The Exception.Message and the Exception.StackTrace will be used automatically</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Error(string source,
                                 Exception exception,
                                 bool throwExceptions)
        {
            SendMessage(ERROR, source, exception.Message, exception.StackTrace, null, throwExceptions);
        }

        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="Exception">Exception. The Exception.Message and the Exception.StackTrace will be used automatically</param>
        /// <param name="data">Parameters list</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Error(string source,
                                 Exception exception,
                                 IDictionary<string, string> data,
                                 bool throwExceptions)
        {
            SendMessage(ERROR, source, exception.Message, exception.StackTrace, data, throwExceptions);
        }

        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Error(string source, 
                                 string message,
                                 IDictionary<string, string> data)
        {
            SendMessage(ERROR, source, message, null, data, false);
        }


        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Error(string source,
                                 string message,
                                 bool throwExceptions)
        {
            SendMessage(ERROR, source, message, null, null, throwExceptions);
        }

        /// <summary>
        /// Log as ERROR
        /// </summary>
        /// <param name="source">Logging Source</param>
        /// <param name="message">Message</param>
        /// <param name="data">Parameters list</param>
        /// <param name="throwExceptions">Flag indicating if the client code want to capture exception from the logging API</param>
        public static void Error(string source,
                                 string message,
                                 IDictionary<string, string> data,
                                 bool throwExceptions)
        {
            SendMessage(ERROR, source, message, null, data, throwExceptions);
        }


        private static void SendMessage(string type, 
                                        string source, 
                                        string message, 
                                        string stackTrace, 
                                        IDictionary<string, string> data, 
                                        bool throwExceptions)
        {
            IConnection rmqConn = null;
            IModel rmqModel = null;
            try
            {
                rmqConn = SuperLoggerHelper.GetRmqConnection();
                rmqModel = rmqConn.CreateModel();
                // @todo: review this
                // This is unecessary and may impact performance. It will require pre rabbit mq configuration to create the Ex
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
                if (throwExceptions)
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
