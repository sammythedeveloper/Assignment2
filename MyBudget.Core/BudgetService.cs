using System;

namespace MyBudget.Core;

public class BudgetService : IBudgetService
{
    private decimal? _monthlyLimit;

    public decimal MonthlyLimit => _monthlyLimit ?? 0m;

    public void SetMonthlyLimit(decimal limit)
    {
        if (limit <= 0)
        {
            throw new InvalidExpenseException("Budget limit must be greater than zero.");
        }
        _monthlyLimit = Math.Round(limit, 2);
    }

    public decimal Remaining(decimal totalSpent)
    {
        return MonthlyLimit - totalSpent;
    }

    public BudgetStatus Evaluate(decimal totalSpent)
    {
        if (_monthlyLimit == null)
        {
            return BudgetStatus.NotSet;
        }

        decimal remaining = Remaining(totalSpent);

        if (remaining < 0)
        {
            return BudgetStatus.OverBudget;
        }

        if (remaining < (MonthlyLimit * 0.10m))
        {
            return BudgetStatus.AlmostOut;
        }

        return BudgetStatus.OnTrack;
    }
}