using System;
using System.IO;
using System.Globalization;
using Xunit;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.ImportExport;

namespace CW_1_CSharp.Tests
{
    public class ImportExportTests : IDisposable
    {
        private readonly string testDbPath = "test_import_db.json";
        private const string tempCsvFile = "temp_test.csv";
        private const string tempJsonFile = "temp_test.json";
        private const string tempYamlFile = "temp_test.yaml";

        // Для CSV-теста нам нужно, чтобы proxy уже имел категории (так как CSV содержит только операции)
        [Fact]
        public void CsvImporter_ShouldImportOperation()
        {
            // Создаем новый DataProxy с пустой базой
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
            var proxy = new DataProxy(testDbPath);

            // Создаем счет и добавляем его
            var account = DomainFactory.CreateBankAccount("CSV Account", 1000);
            proxy.AddBankAccount(account);
            // Добавляем категории вручную (так как CSV не содержит их)
            var expenseCategory = DomainFactory.CreateCategory("expense", "Food");
            proxy.AddCategory(expenseCategory);

            // Формируем CSV: только операция, поля: bank_account_id,amount,date,category_id,description
            string csvContent = "bank_account_id,amount,date,category_id,description\r\n" +
                                $"{account.Id},200,2023-06-01,{expenseCategory.Id},Lunch";
            File.WriteAllText(tempCsvFile, csvContent);

            var importer = new CsvImporter();
            importer.ImportData(tempCsvFile, proxy);

            var ops = proxy.GetAllOperations();
            Assert.Single(ops);
            var updatedAcc = proxy.GetBankAccount(account.Id);
            Assert.Equal(800, updatedAcc.Balance);

            File.Delete(tempCsvFile);
        }

        [Fact]
        public void JsonImporter_ShouldImportData()
        {
            // Создаем новый DataProxy без предварительного добавления категорий,
            // поскольку JSON файл содержит данные для bank_accounts, categories и operations.
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
            var proxy = new DataProxy(testDbPath);

            string jsonContent = @"[
  {
    ""bank_accounts"": [
      { ""id"": 10, ""name"": ""Json Account"", ""balance"": 2000 }
    ],
    ""categories"": [
      { ""id"": 20, ""type"": ""income"", ""name"": ""Salary"" },
      { ""id"": 21, ""type"": ""expense"", ""name"": ""Food"" }
    ],
    ""operations"": [
      { ""id"": 30, ""type"": ""ignored"", ""bank_account_id"": 10, ""amount"": 500, ""date"": ""2023-06-01"", ""category_id"": 21, ""description"": ""Dinner"" }
    ]
  }
]";
            File.WriteAllText(tempJsonFile, jsonContent);

            var importer = new JsonImporter();
            importer.ImportData(tempJsonFile, proxy);

            var accounts = proxy.GetAllBankAccounts();
            var cats = proxy.GetAllCategories();
            var ops = proxy.GetAllOperations();

            // Ожидаем данные из файла:
            // 1 счет, 2 категории, 1 операция
            Assert.Single(accounts);
            Assert.Equal("Json Account", accounts[0].Name);
            Assert.Equal(2, cats.Count);
            Assert.Single(ops);
            // При импорте операция создается с типом, полученным из категории с id=21 (expense)
            Assert.Equal("expense", ops[0].Type);

            File.Delete(tempJsonFile);
        }

        [Fact]
        public void YamlImporter_ShouldImportData()
        {
            // Создаем новый DataProxy без предварительной инициализации (все данные импортируются из YAML)
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
            var proxy = new DataProxy(testDbPath);

            string yamlContent = @"
bank_accounts:
  - id: 100
    name: Yaml Account
    balance: 3000
categories:
  - id: 200
    type: income
    name: Bonus
operations:
  - id: 300
    bank_account_id: 100
    amount: 400
    date: ""2023-06-01""
    category_id: 200
    description: ""Yearly Bonus""
";
            File.WriteAllText(tempYamlFile, yamlContent);

            var importer = new YamlImporter();
            importer.ImportData(tempYamlFile, proxy);

            var accounts = proxy.GetAllBankAccounts();
            var cats = proxy.GetAllCategories();
            var ops = proxy.GetAllOperations();

            Assert.Single(accounts);
            Assert.Equal("Yaml Account", accounts[0].Name);
            Assert.Single(cats);
            Assert.Single(ops);
            Assert.Equal("income", ops[0].Type);

            File.Delete(tempYamlFile);
        }

        public void Dispose()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
        }
    }
}
