namespace TaskFlow.Console;

public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }

    public TaskItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("O título da tarefa não pode ser vazio ou nulo.", nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title.Trim();
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}
