namespace Basic.RabbitMQ;

public class MessageConsumer(RabbitMQClientService rabbitMqClientService) : IMessageConsumer
{
    public IModel Channel(string queueName, string routingKey)
    {
        var channel = rabbitMqClientService.Connect(queueName);

        channel.QueueBind(
            exchange: rabbitMqClientService.BrokerOptions.ExchangeName,
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