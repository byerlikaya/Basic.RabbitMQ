namespace Basic.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRabbitMQClient(this IServiceCollection services, IConfiguration configuration)
    {
        var brokerOptions = configuration.GetSection(nameof(MessageBrokerOptions)).Get<MessageBrokerOptions>();

        services.AddSingleton(_ => new ConnectionFactory
        {
            HostName = brokerOptions.HostName,
            UserName = brokerOptions.Username,
            Password = brokerOptions.Password,
            VirtualHost = brokerOptions.VirtualHost,
            DispatchConsumersAsync = true
        });

        services.AddSingleton<RabbitMQClientService>();
        services.AddSingleton<IMessageProducer, MessageProducer>();
        services.AddSingleton<IMessageConsumer, MessageConsumer>();
    }
}