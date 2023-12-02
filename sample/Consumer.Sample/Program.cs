// See https://aka.ms/new-console-template for more information
IServiceCollection services = new ServiceCollection();
Startup startup = new();
startup.ConfigureServices(services);
IServiceProvider serviceProvider = services.BuildServiceProvider();

var messageConsumer = serviceProvider.GetService<IMessageConsumer>();

var channel = messageConsumer?.Channel("Test_Queue", "Test_Routing_Key");

var basicConsumer = messageConsumer?.GetConsumer(channel);

basicConsumer!.Received += BasicConsumerReceived;

Console.WriteLine(" [>] Waiting for messages.");

Console.WriteLine(" Press [enter] to exit.");

Console.ReadLine();

return;

Task BasicConsumerReceived(object sender, BasicDeliverEventArgs @event)
{
    var message = Encoding.UTF8.GetString(@event.Body.ToArray());

    Console.WriteLine($" [>] Received {message}");

    channel?.BasicAck(@event.DeliveryTag, false);

    return Task.CompletedTask;
}