using System.ComponentModel.DataAnnotations;
using TodoApp.Domain.Entities; 


namespace TodoApp.Tests;


[TestFixture]
public class TaskModelTests
{
    [Test]
    public void TaskModel_TitleCannotBeEmpty()
    {
        // Arrange
        var task = new TodoItem { Title = "" };
        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(task, context, results, true);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(results[0].ErrorMessage, Is.EqualTo("Task title is required"));
    }

    [Test]
    public void TaskModel_ValidTitle_PassesValidation()
    {
        // Arrange
        var task = new TodoItem { Title = "Test Task" };
        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(task, context, results, true);

        // Assert
        Assert.That(isValid, Is.True);
        Assert.That(results, Is.Empty);
    }
}
