using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

public class TodoItem
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Task title is required")]
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}
