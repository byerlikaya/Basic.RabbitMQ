namespace Basic.RabbitMQ;

public class MessageConsumer(
    RabbitMqClientService rabbitMqClientService,
    ConnectionFactory connectionFactory) : IMessageConsumer
{
    private IConnection _connection;

    public IModel Channel(string queueName, string routingKey, ushort prefetchCount = 1)
    {
        _connection ??= connectionFactory.CreateConnection();

        var channel = rabbitMqClientService.Connect(_connection, queueName);
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

    public void Dispose() => _connection?.Dispose();
}