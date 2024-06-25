﻿namespace Consumer.Sample;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        services.AddRabbitMqClient(Configuration);
    }
}