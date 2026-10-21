using TaskFlow.Console;

namespace TaskFlow.Tests;

public class TaskItemTests
{
    [Fact]
    public void Constructor_Should_Create_Task_When_Title_Is_Valid()
    {
        // Arrange & Act
        var task = new TaskItem("Estudar C# e .NET 10");

        // Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal("Estudar C# e .NET 10", task.Title);
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void Constructor_Should_Trim_Title()
    {
        // Arrange & Act
        var task = new TaskItem("  Tarefa com espaços  ");

        // Assert
        Assert.Equal("Tarefa com espaços", task.Title);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Title_Is_Null()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new TaskItem(null!));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Title_Is_Empty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new TaskItem(""));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Title_Is_Whitespace()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new TaskItem("   "));
    }

    [Fact]
    public void Complete_Should_Set_IsCompleted_To_True()
    {
        // Arrange
        var task = new TaskItem("Tarefa pendente");

        // Act
        task.Complete();

        // Assert
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void Constructor_Should_Generate_Unique_Ids()
    {
        // Arrange & Act
        var task1 = new TaskItem("Tarefa 1");
        var task2 = new TaskItem("Tarefa 2");

        // Assert
        Assert.NotEqual(task1.Id, task2.Id);
    }
}
