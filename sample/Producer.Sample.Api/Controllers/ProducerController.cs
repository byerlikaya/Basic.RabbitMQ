namespace Producer.Sample.Api.Controllers;

[ApiController]
// ReSharper disable once HollowTypeName
public class ProducerController(IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("/sendMessage")]
    public async Task<IActionResult> SendMessage(string message)
    {
        await Parallel.ForAsync(0, 10000, (x, _) =>
        {
            messageProducer.SendMessage("Test_Queue", "Test_Routing_Key", $"{message}-{x}");
            return default;
        });

        return Ok();
    }
}