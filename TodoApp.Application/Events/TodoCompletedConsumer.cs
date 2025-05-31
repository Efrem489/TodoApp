using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.Events
{
    public class TodoCompletedConsumer : IConsumer<TodoCompletedEvent>
    {
        private readonly ILogger<TodoCompletedConsumer> _logger;

        public TodoCompletedConsumer(ILogger<TodoCompletedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<TodoCompletedEvent> context)
        {
            _logger.LogInformation("Todo completed: {Id}", context.Message.Id);
            return Task.CompletedTask;
        }
    }
}
