using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.Events
{
    public record TodoCreatedEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
