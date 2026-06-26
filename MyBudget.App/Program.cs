using System;
using MyBudget.Core;

// --- State Variables (Primitive Types - Module 1) ---
decimal totalSpent = 0m;
int expenseCount = 0;
decimal highestExpense = 0m;

decimal foodTotal = 0m;
decimal transportTotal = 0m;
decimal utilitiesTotal = 0m;
decimal entertainmentTotal = 0m;
decimal otherTotal = 0m;

decimal monthlyBudget = 0m;
bool isBudgetSet = false;

// Requirement 3: Raw string literal banner block
string banner = """
============================================================
  MyBudget Expense Tracker
============================================================
""";
Console.WriteLine(banner);

bool running = true;

// Requirement 4: Menu loop driven by a switch statement
while (running)
{
    Console.WriteLine("\n1) Add an expense   2) View summary   3) Set monthly budget   4) Exit");
    Console.Write("> ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ExecuteAddExpense();
            break;
        case "2":
            ExecuteViewSummary();
            break;
        case "3":
            ExecuteSetBudget();
            break;
        case "4":
            running = false;
            Console.WriteLine("Thank you for using MyBudget. Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid option. Please select 1-4.");
            break;
    }
}

// --- Menu Command Functions ---

void ExecuteAddExpense()
{
    // 1. Description tracking
    string description = "";
    while (string.IsNullOrWhiteSpace(description))
    {
        Console.Write("Description : ");
        description = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(description))
            Console.WriteLine("  Description cannot be empty.");
    }

    // 2. Requirement 9 & 11: Amount Input tracking with TryParse + try/catch loop
    decimal amount = 0m;
    while (true)
    {
        Console.Write("Amount      : ");
        if (decimal.TryParse(Console.ReadLine(), out decimal inputVal))
        {
            try
            {
                amount = BudgetRules.ValidateAmount(inputVal);
                break;
            }
            catch (InvalidExpenseException ex)
            {
                Console.WriteLine($"  {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("  Invalid numeric quantity format.");
        }
    }

    // 3. Category Tracking
    string? category = null;
    while (category == null)
    {
        Console.Write("Category    : [Food/Transport/Utilities/Entertainment/Other] ");
        category = BudgetRules.NormalizeCategory(Console.ReadLine());
        if (category == null)
            Console.WriteLine("  Invalid category selected.");
    }

    // 4. Date Tracking
    DateOnly targetDate;
    while (true)
    {
        Console.Write("Date (blank = today): ");
        string? dateStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(dateStr))
        {
            targetDate = DateOnly.FromDateTime(DateTime.Today);
            break;
        }
        if (DateOnly.TryParse(dateStr, out DateOnly parsedDate))
        {
            if (parsedDate > DateOnly.FromDateTime(DateTime.Today))
                Console.WriteLine("  Future dates are rejected.");
            else
            {
                targetDate = parsedDate;
                break;
            }
        }
        else
        {
            Console.WriteLine("  Invalid format. Please use yyyy-mm-dd.");
        }
    }

    // Requirement 12: Handle optional note via Nullable syntax and ??
    Console.Write("Note (optional): ");
    string? rawNote = Console.ReadLine();
    string finalNote = string.IsNullOrWhiteSpace(rawNote) ? "None" : rawNote;

    // Update running statistics metrics state values safely
    totalSpent += amount;
    expenseCount++;
    if (amount > highestExpense) highestExpense = amount;

    switch (category)
    {
        case "Food": foodTotal += amount; break;
        case "Transport": transportTotal += amount; break;
        case "Utilities": utilitiesTotal += amount; break;
        case "Entertainment": entertainmentTotal += amount; break;
        default: otherTotal += amount; break;
    }

    string sizeBand = BudgetRules.ClassifyAmount(amount);
    Console.WriteLine($"  Recorded: {BudgetRules.FormatCurrency(amount)} | {category} | {targetDate:yyyy-MM-dd}");
    Console.WriteLine($"    Size band : {sizeBand}");

    if (isBudgetSet)
    {
        decimal remaining = monthlyBudget - totalSpent;
        string status = BudgetRules.BudgetStatus(remaining, monthlyBudget);
        Console.WriteLine($"  Budget: {BudgetRules.FormatCurrency(remaining)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} -> {status}");
    }
}

void ExecuteViewSummary()
{
    Console.WriteLine("\n--- Summary Report ---");
    if (expenseCount == 0)
    {
        Console.WriteLine("No expenses recorded yet.");
        return;
    }

    decimal average = totalSpent / expenseCount;

    Console.WriteLine($"Count         : {expenseCount}");
    Console.WriteLine($"Total Spent   : {BudgetRules.FormatCurrency(totalSpent)}");
    Console.WriteLine($"Average Spent : {BudgetRules.FormatCurrency(average)}");
    Console.WriteLine($"Highest Single: {BudgetRules.FormatCurrency(highestExpense)}");
    Console.WriteLine("\nCategory Breakdown:");
    Console.WriteLine($"  Food         : {BudgetRules.FormatCurrency(foodTotal)}");
    Console.WriteLine($"  Transport    : {BudgetRules.FormatCurrency(transportTotal)}");
    Console.WriteLine($"  Utilities    : {BudgetRules.FormatCurrency(utilitiesTotal)}");
    Console.WriteLine($"  Entertainment: {BudgetRules.FormatCurrency(entertainmentTotal)}");
    Console.WriteLine($"  Other        : {BudgetRules.FormatCurrency(otherTotal)}");
}

void ExecuteSetBudget()
{
    while (true)
    {
        Console.Write("Monthly budget: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal parsedBudget) && parsedBudget > 0)
        {
            monthlyBudget = parsedBudget;
            isBudgetSet = true;
            Console.WriteLine($"Budget set to {BudgetRules.FormatCurrency(monthlyBudget)}.");
            break;
        }
        Console.WriteLine("  Please provide a positive numerical value.");
    }
}