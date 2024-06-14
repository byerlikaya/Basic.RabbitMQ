namespace Basic.RabbitMQ.Interfaces;

public interface IMessageConsumer
{
    IModel Channel(string queueName, string routingKey);

    AsyncEventingBasicConsumer GetConsumer(IModel channel);
}