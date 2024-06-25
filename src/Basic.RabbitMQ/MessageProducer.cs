namespace Basic.RabbitMQ;

public class MessageProducer(RabbitMqClientService rabbitMqClientService, ConnectionFactory connectionFactory)
    : IMessageProducer, IDisposable
{
    private readonly IConnection _connection = connectionFactory.CreateConnection();

    public void SendMessage(string queueName, string routingKey, string message)
    {
        using var channel = rabbitMqClientService.Connect(_connection, queueName);
        channel.QueueBind(
            queue: queueName,
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            arguments: null);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();

        properties.Persistent = true;
        properties.DeliveryMode = 2;

        channel.BasicPublish(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);
    }

    public void SendMessage<T>(string queueName, string routingKey, T message)
    {
        using var channel = rabbitMqClientService.Connect(_connection, queueName);
        channel.QueueBind(
            queue: queueName,
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            arguments: null);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();

        properties.Persistent = true;
        properties.DeliveryMode = 2;

        channel.BasicPublish(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);

    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}