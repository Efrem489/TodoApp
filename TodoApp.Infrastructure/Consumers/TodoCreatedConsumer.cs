using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Events;

namespace TodoApp.Infrastructure.Consumers
{
    public class TodoCreatedConsumer : IConsumer<TodoCreatedEvent>
    {
        private readonly ILogger<TodoCreatedConsumer> _logger;

        public TodoCreatedConsumer(ILogger<TodoCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<TodoCreatedEvent> context)
        {
            _logger.LogInformation("Todo created: {Id}, Title: {Title}", context.Message.Id, context.Message.Title);
            return Task.CompletedTask;
        }
    }
}
