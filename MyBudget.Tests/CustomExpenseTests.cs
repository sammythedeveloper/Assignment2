using System;
using System.Linq;
using MyBudget.Core;
using MyBudget.Tests.Fakes; 
using Xunit;

namespace MyBudget.Tests;

public class CustomExpenseTests
{
    [Fact]
    public void Add_ValidExpense_IncrementsTotalAndSavesToStore()
    {
        // Arrange
        var fakeStore = new InMemoryExpenseStore();
        var repo = new ExpenseRepository(fakeStore);
        var expense = ExpenseFactory.CreateOneTime("Coffee", 4.50m, ExpenseCategory.Other, DateOnly.FromDateTime(DateTime.Today));

        // Act
        repo.Add(expense);
        repo.Save();

        // Assert
        Assert.Equal(4.50m, repo.Total());
        Assert.Single(fakeStore.Load());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10.50)]
    [InlineData(1000001)]
    public void ValidateAmount_InvalidAmounts_ThrowsInvalidExpenseException(decimal invalidAmount)
    {
        // Arrange, Act & Assert
        Assert.Throws<InvalidExpenseException>(() => ExpenseFactory.ValidateAmount(invalidAmount));
    }

    [Fact]
    public void Evaluate_OverBudgetSpent_ReturnsOverBudgetStatus()
    {
        // Arrange
        var budgetService = new BudgetService();
        budgetService.SetMonthlyLimit(500m);

        // Act
        var status = budgetService.Evaluate(550m);

        // Assert
        Assert.Equal(BudgetStatus.OverBudget, status);
    }
}