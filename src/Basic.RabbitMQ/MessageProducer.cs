namespace Basic.RabbitMQ;

public class MessageProducer(RabbitMQClientService rabbitMqClientService) : IMessageProducer
{
    public void SendMessage(string queueName, string routingKey, string message)
    {
        using var channel = rabbitMqClientService.Connect(queueName);
        channel.QueueBind(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            queue: queueName,
            routingKey: routingKey);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);
    }

    public void SendMessage<T>(string queueName, string routingKey, T message)
    {
        using var channel = rabbitMqClientService.Connect(queueName);
        channel.QueueBind(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            queue: queueName,
            routingKey: routingKey);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();

        properties.Persistent = true;

        channel.BasicPublish(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);
    }
}