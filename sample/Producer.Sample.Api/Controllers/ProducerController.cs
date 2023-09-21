using Basic.RabbitMQ.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Producer.Sample.Api.Controllers
{
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;

        public ProducerController(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        [HttpPost("/sendMessage")]
        public Task<IActionResult> SendEmail(string message)
        {
            _messageProducer.SendMessage("Test_Queue", "Test_Routing_Key", message);
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}