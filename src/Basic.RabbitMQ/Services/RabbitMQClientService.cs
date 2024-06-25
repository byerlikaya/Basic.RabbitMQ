namespace Basic.RabbitMQ.Services;

public class RabbitMqClientService(IConfiguration configuration, ConnectionFactory connectionFactory)
{
    public readonly MessageBrokerOptions BrokerOptions = configuration.GetSection(nameof(MessageBrokerOptions)).Get<MessageBrokerOptions>();

    private IConnection _connection;

    public IModel Connect(string queueName)
    {
        if (_connection is not { IsOpen: true })
            _connection = connectionFactory.CreateConnection();
        return CreateChannel(_connection, queueName);
    }

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