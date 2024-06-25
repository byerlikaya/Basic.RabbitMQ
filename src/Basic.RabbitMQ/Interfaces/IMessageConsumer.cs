namespace Basic.RabbitMQ.Interfaces;

public interface IMessageConsumer
{
    IModel Channel(string queueName, string routingKey, ushort prefetchCount = 10);

    AsyncEventingBasicConsumer GetConsumer(IModel channel);
}