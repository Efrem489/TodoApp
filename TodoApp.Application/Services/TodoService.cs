using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using MassTransit;
using TodoApp.Application.Events;

namespace TodoApp.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICacheService _cacheService;

        public TodoService(ITodoRepository repository, IPublishEndpoint publishEndpoint, ICacheService cacheService)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _cacheService = cacheService;
        }

        public async Task<TodoItemDto> CreateTodoAsync(string title)
        {
            var todo = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(todo);
            await _publishEndpoint.Publish(new TodoCreatedEvent { Id = todo.Id, Title = title });
            await _cacheService.SetAsync($"todo_{todo.Id}", todo, TimeSpan.FromMinutes(10));

            return new TodoItemDto(todo.Id, todo.Title, todo.IsCompleted, todo.CreatedAt);
        }

        public async Task<IEnumerable<TodoItemDto>> GetAllTodosAsync()
        {
            var todos = await _repository.GetAllAsync();
            return todos.Select(t => new TodoItemDto(t.Id, t.Title, t.IsCompleted, t.CreatedAt));
        }

        public async Task<TodoItemDto> GetTodoByIdAsync(Guid id)
        {
            var cached = await _cacheService.GetAsync<TodoItem>($"todo_{id}");
            if (cached != null)
                return new TodoItemDto(cached.Id, cached.Title, cached.IsCompleted, cached.CreatedAt);

            var todo = await _repository.GetByIdAsync(id);
            if (todo == null)
                throw new Exception("Todo not found");

            await _cacheService.SetAsync($"todo_{id}", todo, TimeSpan.FromMinutes(10));
            return new TodoItemDto(todo.Id, todo.Title, todo.IsCompleted, todo.CreatedAt);
        }

        public async Task CompleteTodoAsync(Guid id)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo == null)
                throw new Exception("Todo not found");

            todo.IsCompleted = true;
            await _repository.UpdateAsync(todo);
            await _publishEndpoint.Publish(new TodoCompletedEvent { Id = id });
            await _cacheService.RemoveAsync($"todo_{id}");
        }
    }
}
