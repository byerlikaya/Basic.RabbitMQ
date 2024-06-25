namespace Basic.RabbitMQ.Interfaces;

public interface IMessageConsumer
{
    IModel Channel(string queueName, string routingKey, ushort prefetchCount = 1);

    AsyncEventingBasicConsumer GetConsumer(IModel channel);
}