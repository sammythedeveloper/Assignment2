using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MyBudget.Core;

namespace MyBudget.Data;

public class JsonExpenseStore : IExpenseStore
{
    private readonly string _path;
    private readonly JsonSerializerOptions _options;

    public JsonExpenseStore(string path)
    {
        _path = path;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public IReadOnlyList<Expense> Load()
    {
        if (!File.Exists(_path) || new FileInfo(_path).Length == 0)
        {
            return new List<Expense>();
        }

        string json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<Expense>>(json, _options) ?? new List<Expense>();
    }

    public void Save(IEnumerable<Expense> expenses)
    {
        string json = JsonSerializer.Serialize(expenses, _options);
        File.WriteAllText(_path, json);
    }
}