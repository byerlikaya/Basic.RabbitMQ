# Basic.RabbitMQ
#### .NET library that simplifies RabbitMQ usage and works with the Direct Exchange type.

![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/byerlikaya/Basic.RabbitMQ/dotnet.yml)
[![Basic.RabbitMQ Nuget](https://img.shields.io/nuget/v/Basic.RabbitMQ)](https://www.nuget.org/packages/Basic.RabbitMQ)
[![Basic.RabbitMQ Nuget](https://img.shields.io/nuget/dt/Basic.RabbitMQ)](https://www.nuget.org/packages/Basic.RabbitMQ)

#### Quick Start
The usage of **Basic.RabbitMQ** is quite simple.

1. Install `Basic.RabbitMQ` NuGet package from [here](https://www.nuget.org/packages/Basic.RabbitMQ/).

````
PM> Install-Package Basic.RabbitMQ
````

2. Add services.AddRabbitMQClient(Configuration);

```csharp
builder.Services.AddRabbitMqClient(Configuration);
```

3. Add the necessary information to the `appsettings.json` file.

```json
   "MessageBrokerOptions": {
    "HostName": "",
    "UserName": "",
    "Password": "",
    "ExchangeName": "",
    "VirtualHost": ""
  }
```

4. And start using it. And that's it.

`Producer`

```csharp
[ApiController]
public class ProducerController(IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("/sendMessage")]
    public Task<IActionResult> SendMessage(string message)
    {
        messageProducer.SendMessage("Test_Queue", "Test_Routing_Key", message);
        return Task.FromResult<IActionResult>(Ok());
    }
}
```

`Consumer`

```csharp
var messageConsumer = serviceProvider.GetService<IMessageConsumer>();

var channel = messageConsumer?.Channel("Test_Queue", "Test_Routing_Key");

var basicConsumer = messageConsumer?.GetConsumer(channel);

basicConsumer!.Received += BasicConsumerReceived;

Task BasicConsumerReceived(object sender, BasicDeliverEventArgs @event)
{
    var message = Encoding.UTF8.GetString(@event.Body.ToArray());

    Console.WriteLine($" [>] Received {message}");

    channel?.BasicAck(@event.DeliveryTag, false);

    await Task.Yield();
}
```
