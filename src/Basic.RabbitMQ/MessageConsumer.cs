namespace Basic.RabbitMQ;

public class MessageConsumer(RabbitMqClientService rabbitMqClientService) : IMessageConsumer
{
    public IModel Channel(string queueName, string routingKey, ushort prefetchCount = 1)
    {
        var channel = rabbitMqClientService.Connect(queueName);

        channel.QueueBind(
            queue: queueName,
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            arguments: null);

        channel.BasicQos(0, prefetchCount, false);

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