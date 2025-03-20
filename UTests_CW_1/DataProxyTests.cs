using System;
using System.IO;
using Xunit;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;

namespace CW_1_CSharp.Tests
{
    public class DataProxyTests : IDisposable
    {
        private readonly string testDbPath = "test_database.json";
        private DataProxy proxy;

        public DataProxyTests()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
            proxy = new DataProxy(testDbPath);
        }

        [Fact]
        public void AddIncomeOperation_UpdatesBalance()
        {
            // Arrange
            var account = DomainFactory.CreateBankAccount("Test Account", 1000);
            proxy.AddBankAccount(account);
            var category = DomainFactory.CreateCategory("income", "Salary");
            proxy.AddCategory(category);

            // Act
            var op = DomainFactory.CreateOperation(account.Id, 500, DateTime.Today, category.Id, "Income Test", category.Type);
            proxy.AddOperation(op);
            var updatedAccount = proxy.GetBankAccount(account.Id);

            // Assert
            Assert.Equal(1500, updatedAccount.Balance);
        }

        [Fact]
        public void AddExpenseOperation_PreventsNegativeBalance()
        {
            // Arrange
            var account = DomainFactory.CreateBankAccount("Test Account", 1000);
            proxy.AddBankAccount(account);
            var category = DomainFactory.CreateCategory("expense", "Food");
            proxy.AddCategory(category);

            // Act & Assert
            var op = DomainFactory.CreateOperation(account.Id, 1500, DateTime.Today, category.Id, "Expense Test", category.Type);
            var ex = Assert.Throws<Exception>(() => proxy.AddOperation(op));
            Assert.Contains("Недостаточно средств", ex.Message);
        }

        public void Dispose()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
        }
    }
}