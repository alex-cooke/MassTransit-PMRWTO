using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GettingStarted
{
    public class MessageBase
    {

    }

    public class Message : MessageBase
    {
        public string Text { get; set; }
    }

    public class MessageConsumer :
        IConsumer<Message>
    {
        readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation("Consuming Message: {Text}", context.Message.Text);

            return Task.CompletedTask;
        }
    }

    public class MessageBaseConsumer :
    IConsumer<MessageBase>
    {
        readonly ILogger<MessageBaseConsumer> _logger;

        public MessageBaseConsumer(ILogger<MessageBaseConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MessageBase> context)
        {
            _logger.LogInformation("Consuming Base Message");

            return Task.CompletedTask;
        }
    }
}