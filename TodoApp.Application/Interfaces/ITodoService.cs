using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoService
    {
        Task<TodoItemDto> CreateTodoAsync(string title);
        Task<IEnumerable<TodoItemDto>> GetAllTodosAsync();
        Task<TodoItemDto> GetTodoByIdAsync(Guid id);
        Task CompleteTodoAsync(Guid id);
    }
}
