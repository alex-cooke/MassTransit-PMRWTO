using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace PMRWTO
{
    public class ParentMessage
    {

    }

    public class ChildMessage : ParentMessage
    {
        public string Text { get; set; }
    }

    public class MessageConsumer : IConsumer<ChildMessage>
    {
        readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ChildMessage> context)
        {
            _logger.LogInformation($"Consuming Message : {context.Message.GetType()}");

            return Task.CompletedTask;
        }
    }

    public class ParentMessageConsumer : IConsumer<ParentMessage>
    {
        readonly ILogger<ParentMessageConsumer> _logger;

        public ParentMessageConsumer(ILogger<ParentMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ParentMessage> context)
        {
            _logger.LogInformation($"Consuming Message : {context.Message.GetType()}");

            return Task.CompletedTask;
        }
    }
}