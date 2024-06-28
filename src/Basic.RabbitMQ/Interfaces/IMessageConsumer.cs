namespace Basic.RabbitMQ.Interfaces;

public interface IMessageConsumer : IDisposable
{
    IModel Channel(string queueName, string routingKey, ushort prefetchCount = 1);
    AsyncEventingBasicConsumer GetConsumer(IModel channel);
}