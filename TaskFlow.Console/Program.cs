using TaskFlow.Console;

var manager = new TaskManager();
var running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("========================================");
    Console.WriteLine("           TASKFLOW - Tarefas           ");
    Console.WriteLine("========================================");
    Console.WriteLine(" 1. Adicionar tarefa");
    Console.WriteLine(" 2. Listar tarefas");
    Console.WriteLine(" 3. Concluir tarefa");
    Console.WriteLine(" 0. Sair");
    Console.WriteLine("========================================");
    Console.Write("Escolha uma opção: ");

    var option = Console.ReadLine()?.Trim();

    switch (option)
    {
        case "1":
            AddTask();
            break;
        case "2":
            ListTasks();
            break;
        case "3":
            CompleteTask();
            break;
        case "0":
            running = false;
            break;
        default:
            ShowMessage("Opção inválida. Tente novamente.", ConsoleColor.Red);
            break;
    }
}

Console.WriteLine("\nAté logo!");

void AddTask()
{
    Console.Write("\nTítulo da tarefa: ");
    var title = Console.ReadLine();

    try
    {
        var task = manager.AddTask(title!);
        ShowMessage($"Tarefa \"{task.Title}\" adicionada com sucesso!", ConsoleColor.Green);
    }
    catch (ArgumentException ex)
    {
        ShowMessage($"Erro: {ex.Message}", ConsoleColor.Red);
    }
}

void ListTasks()
{
    Console.WriteLine();

    if (manager.Tasks.Count == 0)
    {
        ShowMessage("Nenhuma tarefa cadastrada.", ConsoleColor.Yellow);
        return;
    }

    Console.WriteLine($"{"#",-4} {"Status",-12} {"ID",-38} Título");
    Console.WriteLine(new string('-', 80));

    int index = 1;
    foreach (var task in manager.Tasks)
    {
        var status = task.IsCompleted ? "[Concluída]" : "[Pendente] ";
        var color = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.White;
        Console.ForegroundColor = color;
        Console.WriteLine($"{index,-4} {status,-12} {task.Id} {task.Title}");
        Console.ResetColor();
        index++;
    }

    Pause();
}

void CompleteTask()
{
    if (manager.Tasks.Count == 0)
    {
        ShowMessage("\nNenhuma tarefa cadastrada.", ConsoleColor.Yellow);
        return;
    }

    Console.Write("\nInforme o ID da tarefa: ");
    var input = Console.ReadLine()?.Trim();

    if (!Guid.TryParse(input, out var id))
    {
        ShowMessage("ID inválido.", ConsoleColor.Red);
        return;
    }

    if (manager.CompleteTask(id))
        ShowMessage("Tarefa concluída com sucesso!", ConsoleColor.Green);
    else
        ShowMessage("Tarefa não encontrada.", ConsoleColor.Red);
}

void ShowMessage(string message, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine($"\n{message}");
    Console.ResetColor();
    Pause();
}

void Pause()
{
    Console.Write("\nPressione qualquer tecla para continuar...");
    Console.ReadKey(intercept: true);
}
