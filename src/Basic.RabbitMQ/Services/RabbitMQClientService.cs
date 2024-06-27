namespace Basic.RabbitMQ.Services;

public class RabbitMqClientService(IConfiguration configuration)
{
    public readonly MessageBrokerOptions BrokerOptions = configuration.GetSection(nameof(MessageBrokerOptions)).Get<MessageBrokerOptions>();

    public IModel Connect(
        IConnection connection,
        string queueName) => CreateChannel(connection, queueName);

    private IModel CreateChannel(IConnection connection, string queueName)
    {
        var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: BrokerOptions.ExchangeName,
            type: ExchangeType.Direct);

        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        return channel;
    }
}