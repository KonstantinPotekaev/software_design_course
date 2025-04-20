using System;
using System.IO;
using Xunit;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Facades;
using CW_1_CSharp.Commands;
using System.Globalization;

namespace CW_1_CSharp.Tests
{
    public class CommandsTests : IDisposable
    {
        private readonly string testDbPath = "test_commands_db.json";
        private DataProxy proxy;
        private BankAccountFacade accFacade;
        private CategoryFacade catFacade;
        private OperationFacade opFacade;

        public CommandsTests()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
            proxy = new DataProxy(testDbPath);
            accFacade = new BankAccountFacade(proxy);
            catFacade = new CategoryFacade(proxy);
            opFacade = new OperationFacade(proxy);
        }

        [Fact]
        public void CreateAccountCommand_ShouldAddAccount()
        {
            var cmd = new CreateAccountCommand(accFacade, "Account1", 1000);
            cmd.Execute();
            var accounts = accFacade.ListAccounts();
            Assert.Single(accounts);
            Assert.Equal("Account1", accounts[0].Name);
            Assert.Equal(1000, accounts[0].Balance);
        }

        [Fact]
        public void UpdateAccountNameCommand_ShouldUpdateName()
        {
            new CreateAccountCommand(accFacade, "OldName", 500).Execute();
            var account = accFacade.ListAccounts()[0];
            new UpdateAccountNameCommand(accFacade, account.Id, "NewName").Execute();
            var updatedAccount = accFacade.ListAccounts()[0];
            Assert.Equal("NewName", updatedAccount.Name);
        }

        [Fact]
        public void DeleteAccountCommand_ShouldRemoveAccount()
        {
            new CreateAccountCommand(accFacade, "AccountToDelete", 500).Execute();
            var account = accFacade.ListAccounts()[0];
            new DeleteAccountCommand(accFacade, account.Id).Execute();
            Assert.Empty(accFacade.ListAccounts());
        }

        [Fact]
        public void CreateCategoryCommand_ShouldAddCategory()
        {
            new CreateCategoryCommand(catFacade, "income", "Salary").Execute();
            var cats = catFacade.ListCategories();
            Assert.Single(cats);
            Assert.Equal("Salary", cats[0].Name);
            Assert.Equal("income", cats[0].Type);
        }

        [Fact]
        public void UpdateCategoryNameCommand_ShouldUpdateCategory()
        {
            new CreateCategoryCommand(catFacade, "expense", "Food").Execute();
            var cat = catFacade.ListCategories()[0];
            new UpdateCategoryNameCommand(catFacade, cat.Id, "Groceries").Execute();
            var updatedCat = catFacade.ListCategories()[0];
            Assert.Equal("Groceries", updatedCat.Name);
        }

        [Fact]
        public void DeleteCategoryCommand_ShouldRemoveCategory()
        {
            new CreateCategoryCommand(catFacade, "expense", "Transport").Execute();
            var cat = catFacade.ListCategories()[0];
            new DeleteCategoryCommand(catFacade, cat.Id).Execute();
            Assert.Empty(catFacade.ListCategories());
        }

        [Fact]
        public void CreateOperationCommand_ShouldAddOperationAndUpdateBalance()
        {
            new CreateAccountCommand(accFacade, "OpAccount", 1000).Execute();
            new CreateCategoryCommand(catFacade, "expense", "Shopping").Execute();
            var account = accFacade.ListAccounts()[0];
            var category = catFacade.ListCategories()[0];
            new CreateOperationCommand(opFacade, account.Id, 300, DateTime.Today.ToString("yyyy-MM-dd"), category.Id, "Buy clothes").Execute();
            var ops = opFacade.ListOperations();
            Assert.Single(ops);
            var updatedAcc = accFacade.ListAccounts()[0];
            Assert.Equal(700, updatedAcc.Balance);
        }

        [Fact]
        public void DeleteOperationCommand_ShouldRemoveOperationAndRestoreBalance()
        {
            new CreateAccountCommand(accFacade, "OpAccount", 1000).Execute();
            new CreateCategoryCommand(catFacade, "expense", "Utilities").Execute();
            var account = accFacade.ListAccounts()[0];
            var category = catFacade.ListCategories()[0];
            new CreateOperationCommand(opFacade, account.Id, 200, DateTime.Today.ToString("yyyy-MM-dd"), category.Id, "Electricity").Execute();
            var op = opFacade.ListOperations()[0];
            new DeleteOperationCommand(opFacade, op.Id).Execute();
            Assert.Empty(opFacade.ListOperations());
            var updatedAcc = accFacade.ListAccounts()[0];
            Assert.Equal(1000, updatedAcc.Balance);
        }

        public void Dispose()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
        }
    }
}
