using TaskFlow.Console;
using Xunit;

namespace TaskFlow.Tests;

public class TaskManagerTests
{
    private readonly TaskManager _manager = new();

    // ---------- AddTask ----------

    [Theory]
    [InlineData("Estudar xUnit", "Estudar xUnit")]
    [InlineData("Comprar pão", "Comprar pão")]
    [InlineData("  Com espaços  ", "Com espaços")]
    [InlineData("a", "a")]
    public void AddTask_ComTituloValido_DeveRetornarTarefaComTituloTratado(string entrada, string esperado)
    {
        var task = _manager.AddTask(entrada);

        Assert.NotNull(task);
        Assert.Equal(esperado, task.Title);
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void AddTask_DeveAdicionarTarefaNaColecao()
    {
        var task = _manager.AddTask("Nova tarefa");

        Assert.Single(_manager.Tasks);
        Assert.Contains(task, _manager.Tasks);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t\n")]
    public void AddTask_ComTituloInvalido_DeveLancarArgumentException(string? titulo)
    {
        Assert.Throws<ArgumentException>(() => _manager.AddTask(titulo!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddTask_ComTituloInvalido_NaoDeveAdicionarNaColecao(string? titulo)
    {
        try
        {
            _manager.AddTask(titulo!);
        }
        catch (ArgumentException)
        {
            // esperado
        }

        Assert.Empty(_manager.Tasks);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    public void AddTask_VariasTarefas_DeveManterQuantidadeCorreta(int quantidade)
    {
        for (var i = 0; i < quantidade; i++)
        {
            _manager.AddTask($"Tarefa {i}");
        }

        Assert.Equal(quantidade, _manager.Tasks.Count);
    }

    [Fact]
    public void AddTask_VariasTarefas_DeveGerarIdsUnicos()
    {
        _manager.AddTask("Tarefa 1");
        _manager.AddTask("Tarefa 2");
        _manager.AddTask("Tarefa 3");

        var ids = _manager.Tasks.Select(t => t.Id).ToList();

        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void AddTask_DeveManterOrdemDeInsercao()
    {
        var t1 = _manager.AddTask("Primeira");
        var t2 = _manager.AddTask("Segunda");

        Assert.Equal(new[] { t1, t2 }, _manager.Tasks);
    }

    [Fact]
    public void AddTask_TitulosRepetidos_DevePermitirDuplicatas()
    {
        var t1 = _manager.AddTask("Repetida");
        var t2 = _manager.AddTask("Repetida");

        Assert.Equal(2, _manager.Tasks.Count);
        Assert.NotSame(t1, t2);
    }

    // ---------- Tasks ----------

    [Fact]
    public void Tasks_SemTarefas_DeveRetornarColecaoVazia()
    {
        Assert.Empty(_manager.Tasks);
    }

    [Fact]
    public void Tasks_DeveSerSomenteLeitura()
    {
        _manager.AddTask("Tarefa");

        var colecao = _manager.Tasks as ICollection<TaskItem>;

        Assert.True(colecao is null || colecao.IsReadOnly);
    }

    // ---------- GetById ----------

    [Fact]
    public void GetById_ComIdExistente_DeveRetornarMesmaInstancia()
    {
        var task = _manager.AddTask("Tarefa");

        var result = _manager.GetById(task.Id);

        Assert.Same(task, result);
    }

    [Fact]
    public void GetById_ComIdInexistente_DeveRetornarNull()
    {
        _manager.AddTask("Tarefa");

        Assert.Null(_manager.GetById(Guid.NewGuid()));
    }

    [Fact]
    public void GetById_ComGuidEmpty_DeveRetornarNull()
    {
        _manager.AddTask("Tarefa");

        Assert.Null(_manager.GetById(Guid.Empty));
    }

    [Fact]
    public void GetById_SemTarefas_DeveRetornarNull()
    {
        Assert.Null(_manager.GetById(Guid.NewGuid()));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void GetById_ComVariasTarefas_DeveRetornarATarefaCorreta(int indice)
    {
        var tarefas = new[]
        {
            _manager.AddTask("A"),
            _manager.AddTask("B"),
            _manager.AddTask("C")
        };

        var result = _manager.GetById(tarefas[indice].Id);

        Assert.Same(tarefas[indice], result);
    }

    // ---------- CompleteTask ----------

    [Fact]
    public void CompleteTask_ComIdExistente_DeveRetornarTrueEConcluirTarefa()
    {
        var task = _manager.AddTask("Tarefa");

        var result = _manager.CompleteTask(task.Id);

        Assert.True(result);
        Assert.True(task.IsCompleted);
    }

    [Theory]
    [InlineData(false)] // Guid aleatório
    [InlineData(true)]  // Guid.Empty
    public void CompleteTask_ComIdInexistente_DeveRetornarFalseENaoAlterarTarefas(bool usarGuidEmpty)
    {
        var task = _manager.AddTask("Tarefa");
        var idInvalido = usarGuidEmpty ? Guid.Empty : Guid.NewGuid();

        var result = _manager.CompleteTask(idInvalido);

        Assert.False(result);
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void CompleteTask_SemTarefas_DeveRetornarFalse()
    {
        Assert.False(_manager.CompleteTask(Guid.NewGuid()));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void CompleteTask_DeveConcluirApenasATarefaInformada(int indice)
    {
        var tarefas = new[]
        {
            _manager.AddTask("A"),
            _manager.AddTask("B"),
            _manager.AddTask("C")
        };

        _manager.CompleteTask(tarefas[indice].Id);

        for (var i = 0; i < tarefas.Length; i++)
        {
            Assert.Equal(i == indice, tarefas[i].IsCompleted);
        }
    }

    [Fact]
    public void CompleteTask_TarefaJaConcluida_DeveRetornarTrueEManterConcluida()
    {
        var task = _manager.AddTask("Tarefa");
        _manager.CompleteTask(task.Id);

        var result = _manager.CompleteTask(task.Id);

        Assert.True(result);
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void CompleteTask_NaoDeveRemoverTarefaDaColecao()
    {
        var task = _manager.AddTask("Tarefa");

        _manager.CompleteTask(task.Id);

        Assert.Single(_manager.Tasks);
        Assert.Contains(task, _manager.Tasks);
    }
}