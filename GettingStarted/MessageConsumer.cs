using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GettingStarted
{
    public interface IMessage
    {

    }

    public class Message : IMessage
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

    public class AllMessageConsumer :
    IConsumer<IMessage>
    {
        readonly ILogger<AllMessageConsumer> _logger;

        public AllMessageConsumer(ILogger<AllMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IMessage> context)
        {
            _logger.LogInformation("Consuming IMessage");

            return Task.CompletedTask;
        }
    }
}