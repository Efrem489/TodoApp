using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] string title)
        {
            var todo = await _todoService.CreateTodoAsync(title);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAll()
        {
            var todos = await _todoService.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetById(Guid id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            return Ok(todo);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _todoService.CompleteTodoAsync(id);
            return NoContent();
        }
    }
}
