using System;

namespace MyBudget.Core;

public abstract class Expense : IReportable
{
    public Guid Id { get; }
    public string Description { get; }
    public decimal Amount { get; }
    public ExpenseCategory Category { get; }
    public DateTime Date { get; }

    protected Expense(Guid id, string description, decimal amount, ExpenseCategory category, DateTime date)
    {
        Id = id;
        Description = description;
        Amount = amount;
        Category = category;
        Date = date;
    }

    public abstract decimal MonthlyImpact { get; }

    public abstract string ToReportLine();
}