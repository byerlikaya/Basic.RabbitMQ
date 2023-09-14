using Basic.RabbitMQ.Interfaces;
using Basic.RabbitMQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Basic.RabbitMQ
{
    public class MessageProducer : IMessageProducer
    {
        private readonly RabbitMQClientService _rabbitMqClientService;

        public MessageProducer(RabbitMQClientService rabbitMqClientService)
        {
            _rabbitMqClientService = rabbitMqClientService;
        }

        public void SendMessage(string queueName, string routingKey, string message)
        {
            var channel = _rabbitMqClientService.Connect(queueName);

            channel.QueueBind(
                exchange: _rabbitMqClientService.BrokerOptions.ExchangeName,
                queue: queueName,
                routingKey: routingKey);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: _rabbitMqClientService.BrokerOptions.ExchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
        }

        public void SendMessage<T>(string queueName, string routingKey, T message)
        {
            var channel = _rabbitMqClientService.Connect(queueName);

            channel.QueueBind(
                exchange: _rabbitMqClientService.BrokerOptions.ExchangeName,
                queue: queueName,
                routingKey: routingKey);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(
                exchange: _rabbitMqClientService.BrokerOptions.ExchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
        }
    }
}
