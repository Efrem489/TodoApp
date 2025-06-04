using System;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Services;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Application.DTOs;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
namespace TodoApp.Tests;

[TestFixture]
public class TodoServiceTests
{
    private Mock<ITodoRepository> _todoRepositoryMock;
    private Mock<IPublishEndpoint> _publishEndpointMock;
    private Mock<ICacheService> _cacheServiceMock;
    private TodoService _todoService;

    [SetUp]
    public void Setup()
    {
        _todoRepositoryMock = new Mock<ITodoRepository>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _cacheServiceMock = new Mock<ICacheService>();
        _todoService = new TodoService(_todoRepositoryMock.Object, _publishEndpointMock.Object, _cacheServiceMock.Object);
    }

    [Test]
    public async Task CreateTodoAsync_ShouldCreateTodo_AndPublishEvent()
    {
        // Arrange
        var title = "Test Task";
        _todoRepositoryMock.Setup(r => r.AddAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);
        _publishEndpointMock.Setup(p => p.Publish(It.IsAny<object>(), default)).Returns(Task.CompletedTask);
        _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<TodoItem>(), It.IsAny<TimeSpan>())).Returns(Task.CompletedTask);

        // Act
        var result = await _todoService.CreateTodoAsync(title);

        // Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(title, result.Title);
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(result.IsCompleted);
        _todoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Once());
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<object>(), default), Times.Once());
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<TodoItem>(), It.IsAny<TimeSpan>()), Times.Once());
    }
}
