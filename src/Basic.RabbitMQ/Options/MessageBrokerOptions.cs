namespace Basic.RabbitMQ.Options;

public class MessageBrokerOptions
{
    public string HostName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ExchangeName { get; set; }
    public string VirtualHost { get; set; }
    public int Port { get; set; }
}