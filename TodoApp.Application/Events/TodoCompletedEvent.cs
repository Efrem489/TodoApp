using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.Events
{
    public record TodoCompletedEvent
    {
        public Guid Id { get; set; }
    }
}
