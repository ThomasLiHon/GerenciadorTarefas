using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Tarefa
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
}

class Program
{
    static List<Tarefa> tarefas = new List<Tarefa>();
    static string arquivo = "tarefas.json";
    static int proximoId = 1;

    static void Main()
    {
        CarregarTarefas();

        bool sair = false;
        while (!sair)
        {
            Console.WriteLine("\n===== GERENCIADOR DE TAREFAS =====");
            Console.WriteLine("1. Adicionar tarefa");
            Console.WriteLine("2. Listar pendentes");
            Console.WriteLine("3. Listar concluídas");
            Console.WriteLine("4. Marcar como concluída");
            Console.WriteLine("5. Remover tarefa");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    AdicionarTarefa();
                    break;
                case "2":
                    ListarTarefas(false);
                    break;
                case "3":
                    ListarTarefas(true);
                    break;
                case "4":
                    MarcarComoConcluida();
                    break;
                case "5":
                    RemoverTarefa();
                    break;
                case "0":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void AdicionarTarefa()
    {
        Console.Write("Descrição: ");
        string desc = Console.ReadLine();
        tarefas.Add(new Tarefa { Id = proximoId++, Descricao = desc, Concluida = false });
        SalvarTarefas();
    }

    static void ListarTarefas(bool concluidas)
    {
        var lista = tarefas.Where(t => t.Concluida == concluidas);
        Console.WriteLine(concluidas ? "\n--- Concluídas ---" : "\n--- Pendentes ---");
        foreach (var t in lista)
            Console.WriteLine($"{t.Id}: {t.Descricao}");
    }

    static void MarcarComoConcluida()
    {
        Console.Write("ID da tarefa: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa != null)
            {
                tarefa.Concluida = true;
                SalvarTarefas();
            }
        }
    }

    static void RemoverTarefa()
    {
        Console.Write("ID da tarefa: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            tarefas.RemoveAll(t => t.Id == id);
            SalvarTarefas();
        }
    }

    static void SalvarTarefas()
    {
        var json = JsonSerializer.Serialize(tarefas, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(arquivo, json);
    }

    static void CarregarTarefas()
    {
        if (File.Exists(arquivo))
        {
            string json = File.ReadAllText(arquivo);
            tarefas = JsonSerializer.Deserialize<List<Tarefa>>(json);
            proximoId = tarefas.Any() ? tarefas.Max(t => t.Id) + 1 : 1;
        }
    }
}
