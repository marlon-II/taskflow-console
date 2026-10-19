namespace TaskFlow.Console;

public class TaskManager
{
    private readonly List<TaskItem> _tasks = new();

    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    public TaskItem AddTask(string title)
    {
        var task = new TaskItem(title);
        _tasks.Add(task);
        return task;
    }

    public TaskItem? GetById(Guid id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public bool CompleteTask(Guid id)
    {
        var task = GetById(id);
        if (task == null)
        {
            return false;
        }

        task.Complete();
        return true;
    }
}
