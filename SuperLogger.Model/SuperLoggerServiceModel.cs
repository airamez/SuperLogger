using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

using SuperLogger.Helper;

namespace SuperLogger.Model
{
    public class SuperLoggerServiceModel
    {
        public static bool Running { set; get; }
        private IConnection _rmqConnection { set; get; }
        private IModel _rmqChannel;

        public SuperLoggerServiceModel()
        {
            Running = true;
        }

        public void Run()
        {
            _rmqConnection = SuperLoggerHelper.GetRmqConnection();
            _rmqChannel = _rmqConnection.CreateModel();
//            _rmqChannel.QueueDeclare(SuperLoggerHelper.QueueName, true, false, false, null);
            QueueingBasicConsumer rmqConsumer = new QueueingBasicConsumer();
            _rmqChannel.BasicConsume(SuperLoggerHelper.QueueName, false, rmqConsumer);
            while (Running)
            {
                //@todo: Catch exception from RMQ and reconnect
                BasicDeliverEventArgs message = rmqConsumer.Queue.Dequeue();
                try
                {
                    var body = message.Body;
                    var payload = Encoding.UTF8.GetString(body);
                    RmqMessageContent messageContent = JsonConvert.DeserializeObject<RmqMessageContent>(payload);
                    SuperLoggerDbModel dbModel = new SuperLoggerDbModel();
                    dbModel.AddLogEntry(messageContent.Source,
                                        messageContent.LogType,
                                        messageContent.CorrelationID,
                                        JsonConvert.DeserializeObject<DateTime>(messageContent.CreatedOn),
                                        messageContent.Message,
                                        messageContent.StackTrace,
                                        messageContent.Data);
                    _rmqChannel.BasicAck(message.DeliveryTag, false);
                } catch (Exception ex)
                {
                    _rmqChannel.BasicReject(message.DeliveryTag, true);
                }
            }
        }
    }
}
