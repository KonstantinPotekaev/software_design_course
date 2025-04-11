using System;
using System.IO;
using Xunit;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Facades;

namespace CW_1_CSharp.Tests
{
    public class AnalyticsTests : IDisposable
    {
        private readonly string testDbPath = "test_database.json";
        private DataProxy proxy;
        private AnalyticsFacade analytics;
        private BankAccountFacade accFacade;
        private OperationFacade opFacade;
        private CategoryFacade catFacade;

        public AnalyticsTests()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
            proxy = new DataProxy(testDbPath);
            analytics = new AnalyticsFacade(proxy);
            accFacade = new BankAccountFacade(proxy);
            opFacade = new OperationFacade(proxy);
            catFacade = new CategoryFacade(proxy);
        }

        [Fact]
        public void GetIncomeExpenseDiff_ShouldReturnCorrectDifference()
        {
            // Arrange
            var account = DomainFactory.CreateBankAccount("Test", 0);
            proxy.AddBankAccount(account);
            var incomeCategory = DomainFactory.CreateCategory("income", "Salary");
            var expenseCategory = DomainFactory.CreateCategory("expense", "Food");
            proxy.AddCategory(incomeCategory);
            proxy.AddCategory(expenseCategory);

            DateTime today = DateTime.Today;
            var opIncome = DomainFactory.CreateOperation(account.Id, 1000, today, incomeCategory.Id, "Income", incomeCategory.Type);
            proxy.AddOperation(opIncome);
            var opExpense = DomainFactory.CreateOperation(account.Id, 300, today, expenseCategory.Id, "Expense", expenseCategory.Type);
            proxy.AddOperation(opExpense);

            // Act
            double diff = analytics.GetIncomeExpenseDiff(today.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));

            // Assert
            Assert.Equal(700, diff);
        }

        [Fact]
        public void GroupByCategory_ShouldReturnCorrectGrouping()
        {
            // Arrange
            var account = DomainFactory.CreateBankAccount("Test", 0);
            proxy.AddBankAccount(account);
            var incomeCategory = DomainFactory.CreateCategory("income", "Salary");
            var expenseCategory = DomainFactory.CreateCategory("expense", "Food");
            proxy.AddCategory(incomeCategory);
            proxy.AddCategory(expenseCategory);

            DateTime today = DateTime.Today;
            var opIncome = DomainFactory.CreateOperation(account.Id, 1000, today, incomeCategory.Id, "Income", incomeCategory.Type);
            proxy.AddOperation(opIncome);
            var opExpense = DomainFactory.CreateOperation(account.Id, 200, today, expenseCategory.Id, "Expense", expenseCategory.Type);
            proxy.AddOperation(opExpense);

            // Act
            var groups = analytics.GroupByCategory(today.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));

            // Assert
            // Ожидаем две группы: одна для доходов и одна для расходов
            Assert.Equal(2, groups.Count);
        }

        public void Dispose()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
        }
    }
}
