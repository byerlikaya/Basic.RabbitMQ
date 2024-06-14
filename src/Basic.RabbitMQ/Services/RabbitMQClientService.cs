namespace Basic.RabbitMQ.Services;

// ReSharper disable once InconsistentNaming
public class RabbitMQClientService(IConfiguration configuration, ConnectionFactory connectionFactory) : IDisposable
{
    public readonly MessageBrokerOptions BrokerOptions = configuration.GetSection(nameof(MessageBrokerOptions)).Get<MessageBrokerOptions>();

    private IConnection _connection;
    private IModel _channel;

    public IModel Connect(string queueName)
    {
        if (_channel is { IsOpen: true } && _channel.CurrentQueue == queueName)
            return _channel;

        if (_connection is not { IsOpen: true })
            _connection = connectionFactory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: BrokerOptions.ExchangeName,
            type: "direct",
            durable: true,
            autoDelete: false);

        _channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        return _channel;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();

        _connection?.Close();
        _connection?.Dispose();
    }
}