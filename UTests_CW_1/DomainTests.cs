using System;
using Xunit;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;

namespace CW_1_CSharp.Tests
{
    public class DomainTests
    {
        [Fact]
        public void CreateBankAccount_ShouldSetCorrectProperties()
        {
            // Arrange & Act
            var account = DomainFactory.CreateBankAccount("Test Account", 1000);

            // Assert
            Assert.Equal("Test Account", account.Name);
            Assert.Equal(1000, account.Balance);
        }

        [Fact]
        public void UpdateBalance_ShouldUpdateCorrectly()
        {
            // Arrange
            var account = DomainFactory.CreateBankAccount("Test Account", 500);

            // Act
            account.UpdateBalance(200);

            // Assert
            Assert.Equal(700, account.Balance);
        }

        [Fact]
        public void CreateOperation_ShouldSetTypeFromCategory()
        {
            // Arrange
            var category = DomainFactory.CreateCategory("income", "Salary");
            var today = DateTime.Today;

            // Act
            var op = DomainFactory.CreateOperation(1, 1000, today, category.Id, "Test Operation", category.Type);

            // Assert
            Assert.Equal("income", op.Type);
        }
    }
}