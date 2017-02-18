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
        private IConnection _rmqConnection { set; get; }

        public SuperLoggerServiceModel ()
        {
            PrepareRMQ();
        }

        private void PrepareRMQ()
        {
            //_rmqConnection = SuperLoggerHelper.GetRmqConnection();
            //IModel rmqChannel = _rmqConnection.CreateModel();
            //rmqChannel.QueueDeclare(SuperLoggerHelper.QueueName, true, false, false, null);
            //var rmqConsumer = new QueueingBasicConsumer(rmqChannel);
            //rmqChannel.BasicConsume(SuperLoggerHelper.QueueName, false, rmqConsumer);
            //var message = rmqConsumer.Queue.Dequeue();

            //byte[] body = message.Body;
            //var payload = Encoding.UTF8.GetString(body);

            //var rmqConsumer = new EventingBasicConsumer(rmqChannel);
            //rmqConsumer.Received += (model, ea) =>
            //{
            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);
            //};

        }
    }
}
