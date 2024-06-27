namespace Producer.Sample.Api.Controllers;

[ApiController]
// ReSharper disable once HollowTypeName
public class ProducerController(IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("/sendMessageMultiple")]
    public async Task<IActionResult> SendMessageMultiple(string message)
    {
        await Parallel.ForAsync(0, 10000, (x, _) =>
        {
            messageProducer.SendMessage("Test_Queue", "Test_Routing_Key", $"{message}-{x}");
            return default;
        });

        return Ok();
    }

    [HttpPost("/sendMessage")]
    public Task<IActionResult> SendMessage(string message)
    {
        messageProducer.SendMessage("Test_Queue", "Test_Routing_Key", $"{message}");
        return Task.FromResult<IActionResult>(Ok());
    }
}