using System;

namespace MyBudget.Core;

public static class BudgetRules
{
    // Requirement 10: Guard clauses + throw custom exception
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidExpenseException("Amount must be greater than zero.");
        return amount;
    }

    // Requirement 5 & 8: Switch expression + relational pattern matching
    public static string ClassifyAmount(decimal amount) => amount switch
    {
        < 20m => "Small",
        >= 20m and < 100m => "Medium",
        _ => "Large"
    };

    // Requirement 6: Demonstrates Range (..) and Index from end (^)
    public static string? NormalizeCategory(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        string clean = input.Trim();
        if (clean.Length == 0) return null;

        // Formats string to Proper Case using Range and Index from end
        string properCase = clean[..1].ToUpper() + clean[1..].ToLower();

        return properCase switch
        {
            "Food" or "Transport" or "Utilities" or "Entertainment" or "Other" => properCase,
            _ => null
        };
    }

    // Requirement 5: Switch expression for budget tracking
    public static string BudgetStatus(decimal remaining, decimal monthlyLimit)
    {
        if (monthlyLimit <= 0) return "On track";
        
        decimal ratio = remaining / monthlyLimit;

        return ratio switch
        {
            < 0m => "Over budget",
            >= 0m and < 0.10m => "Almost out",
            _ => "On track"
        };
    }

    // Requirement 7: Well-named method overloading
    public static string FormatCurrency(decimal amount) => $"${amount:N2}";

    public static string FormatCurrency(decimal amount, string prefix) => $"{prefix}{amount:N2}";
}