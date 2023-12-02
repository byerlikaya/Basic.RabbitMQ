namespace Basic.RabbitMQ.Interfaces;

public interface IMessageProducer
{
    void SendMessage(string queueName, string routingKey, string message);
    void SendMessage<T>(string queueName, string routingKey, T message);
}