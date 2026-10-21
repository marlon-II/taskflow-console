# TaskFlow Console

Gerenciador de tarefas (To-Do List) via console, desenvolvido em .NET com testes unitários em xUnit.

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) ou superior

Verifique a instalação:

```bash
dotnet --version
```

## Estrutura do Projeto

```
taskflow-console/
├── TaskFlow.Console/   # Aplicação console
└── TaskFlow.Tests/     # Testes unitários (xUnit)
```

## Como Compilar

Na raiz da solution, execute:

```bash
dotnet build
```

## Como Rodar a Aplicação

```bash
dotnet run --project TaskFlow.Console
```

O menu interativo será exibido no terminal com as opções:

```
========================================
           TASKFLOW - Tarefas
========================================
 1. Adicionar tarefa
 2. Listar tarefas
 3. Concluir tarefa
 0. Sair
========================================
```

## Como Executar os Testes

Para rodar todos os testes unitários:

```bash
dotnet test
```

Para exibir detalhes de cada teste:

```bash
dotnet test --verbosity normal
```

Para rodar apenas os testes de um projeto específico:

```bash
dotnet test TaskFlow.Tests
```
