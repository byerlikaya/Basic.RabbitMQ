namespace Basic.RabbitMQ.Extensions;

// ReSharper disable once UnusedType.Global
public static class ServiceCollectionExtensions
{
    // ReSharper disable once UnusedMember.Global
    public static void AddRabbitMqClient(
        this IServiceCollection services,
        IConfiguration configuration,
        ServiceLifetime messageConsumerServiceLifetime = ServiceLifetime.Singleton) =>
        CreateServices(services, configuration.GetSection(nameof(MessageBrokerOptions)).Get<MessageBrokerOptions>(), messageConsumerServiceLifetime);

    // ReSharper disable once UnusedMember.Global
    public static void AddRabbitMqClient(
        this IServiceCollection services,
        MessageBrokerOptions messageBrokerOptions,
        ServiceLifetime messageConsumerServiceLifetime = ServiceLifetime.Singleton) =>
        CreateServices(services, messageBrokerOptions, messageConsumerServiceLifetime);

    private static void CreateServices(
        IServiceCollection services,
        MessageBrokerOptions messageBrokerOptions,
        ServiceLifetime messageConsumerServiceLifetime)
    {
        services.AddSingleton(_ => new ConnectionFactory
        {
            HostName = messageBrokerOptions.HostName,
            UserName = messageBrokerOptions.Username,
            Password = messageBrokerOptions.Password,
            VirtualHost = messageBrokerOptions.VirtualHost,
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
        });

        services.AddSingleton<RabbitMqClientService>();
        services.AddSingleton<IMessageConsumer, MessageConsumer>();
        CreateConsumerService(services, messageConsumerServiceLifetime);
    }

    private static void CreateConsumerService(
        IServiceCollection services,
        ServiceLifetime messageConsumerServiceLifetime)
    {
        switch (messageConsumerServiceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<IMessageProducer, MessageProducer>();
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<IMessageProducer, MessageProducer>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<IMessageProducer, MessageProducer>();
                break;
            default:
                services.AddSingleton<IMessageProducer, MessageProducer>();
                break;
        }
    }
}