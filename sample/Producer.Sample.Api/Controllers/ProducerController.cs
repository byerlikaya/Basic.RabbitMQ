namespace Producer.Sample.Api.Controllers;

[ApiController]
// ReSharper disable once HollowTypeName
public class ProducerController(IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("/sendMessage")]
    public Task<IActionResult> SendEmail(string message)
    {
        messageProducer.SendMessage("Test_Queue", "Test_Routing_Key", message);
        return Task.FromResult<IActionResult>(Ok());
    }
}