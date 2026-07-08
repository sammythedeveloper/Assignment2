using System;

namespace MyBudget.Core;

public static class ExpenseFactory
{
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0 || amount > 1000000)
        {
            throw new InvalidExpenseException("Amount must be greater than 0 and less than or equal to 1,000,000.");
        }
        return Math.Round(amount, 2);
    }

    public static OneTimeExpense CreateOneTime(
        string description, 
        decimal amount,
        ExpenseCategory category, 
        DateOnly date)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description cannot be blank.");
        }

        decimal validatedAmount = ValidateAmount(amount);
        // Added .Trim() here to fix the failing test case
        return new OneTimeExpense(Guid.NewGuid(), description.Trim(), validatedAmount, category, date);
    }

    public static RecurringExpense CreateRecurring(
        string description, 
        decimal amount,
        ExpenseCategory category, 
        DateOnly date, 
        int timesPerMonth)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description cannot be blank.");
        }
        if (timesPerMonth < 1)
        {
            throw new InvalidExpenseException("Times per month must be at least 1.");
        }

        decimal validatedAmount = ValidateAmount(amount);
        // Added .Trim() here as a defensive programming best practice
        return new RecurringExpense(Guid.NewGuid(), description.Trim(), validatedAmount, category, date, timesPerMonth);
    }
}