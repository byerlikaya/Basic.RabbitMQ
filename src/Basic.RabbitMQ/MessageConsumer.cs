using Basic.RabbitMQ.Interfaces;
using Basic.RabbitMQ.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Basic.RabbitMQ
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly RabbitMQClientService _rabbitMqClientService;

        public MessageConsumer(RabbitMQClientService rabbitMqClientService)
        {
            _rabbitMqClientService = rabbitMqClientService;
        }

        public IModel Channel(string queueName, string routingKey)
        {
            var channel = _rabbitMqClientService.Connect(queueName);

            channel.QueueBind(
                exchange: _rabbitMqClientService.BrokerOptions.ExchangeName,
                queue: queueName,
                routingKey: routingKey);

            channel.BasicQos(0, 10, false);

            return channel;
        }

        public AsyncEventingBasicConsumer GetConsumer(IModel channel)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);

            channel.BasicConsume(
                queue: channel.CurrentQueue,
                autoAck: false,
                consumer: consumer);

            return consumer;
        }
    }
}
