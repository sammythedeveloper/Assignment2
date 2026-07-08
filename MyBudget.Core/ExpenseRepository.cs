using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBudget.Core;

public class ExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> _expenses = new();
    private readonly IExpenseStore _store;

    public ExpenseRepository(IExpenseStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _expenses = _store.Load().ToList();
    }

    public IReadOnlyList<Expense> GetAll()
    {
        return _expenses.OrderBy(e => e.Date).ToList();
    }

    public void Add(Expense expense)
    {
        if (expense == null)
        {
            throw new ArgumentNullException(nameof(expense));
        }
        _expenses.Add(expense);
    }

    public decimal Total()
    {
        return _expenses.Sum(e => e.MonthlyImpact);
    }

    public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
    {
        return _expenses
            .GroupBy(e => e.Category)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.MonthlyImpact));
    }

    public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
    {
        return _expenses
            .Where(e => e.Category == category)
            .OrderBy(e => e.Date)
            .ToList();
    }

    public void Save()
    {
        _store.Save(_expenses);
    }
}