using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;

namespace CW_1_CSharp.Proxies
{
    public class DataProxy
    {
        private readonly string _dbPath;
        private Dictionary<int, BankAccount> _bankAccounts;
        private Dictionary<int, Category> _categories;
        private Dictionary<int, Operation> _operations;

        public DataProxy(string dbPath = "database.json")
        {
            _dbPath = dbPath;
            _bankAccounts = new Dictionary<int, BankAccount>();
            _categories = new Dictionary<int, Category>();
            _operations = new Dictionary<int, Operation>();

            if (File.Exists(_dbPath))
            {
                LoadFromFile();
            }
            else
            {
                // Создадим новый пустой JSON
                var initData = new
                {
                    BankAccounts = new List<object>(),
                    Categories = new List<object>(),
                    Operations = new List<object>(),
                    NextIds = new int[] { 1, 1, 1 }
                };
                var json = JsonSerializer.Serialize(initData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_dbPath, json);
            }
        }

        private void LoadFromFile()
        {
            var text = File.ReadAllText(_dbPath);
            var doc = JsonSerializer.Deserialize<DatabaseDto>(text);
            if (doc == null) return;

            if (doc.NextIds != null && doc.NextIds.Length == 3)
            {
                DomainFactory.SetNextIds(doc.NextIds[0], doc.NextIds[1], doc.NextIds[2]);
            }

            _bankAccounts.Clear();
            if (doc.BankAccounts != null)
            {
                foreach (var ba in doc.BankAccounts)
                {
                    var bankAccount = new BankAccount(ba.Id, ba.Name, ba.Balance);
                    _bankAccounts[bankAccount.Id] = bankAccount;
                }
            }

            _categories.Clear();
            if (doc.Categories != null)
            {
                foreach (var c in doc.Categories)
                {
                    var category = new Category(c.Id, c.Type, c.Name);
                    _categories[category.Id] = category;
                }
            }

            // Загружаем Operations
            _operations.Clear();
            if (doc.Operations != null)
            {
                foreach (var o in doc.Operations)
                {
                    var date = DateTime.Parse(o.Date);
                    if (_categories.ContainsKey(o.CategoryId))
                    {
                        var category = _categories[o.CategoryId];
                        var operation = new Operation(o.Id, o.BankAccountId, o.Amount, date, o.CategoryId,
                            o.Description, category.Type);
                        _operations[operation.Id] = operation;
                    }
                    else
                    {
                        Console.WriteLine($"Категория с ID {o.CategoryId} не найдена для операции ID {o.Id}.");
                    }
                }
            }
        }

        private void SaveToFile()
        {
            var (nextBA, nextCat, nextOp) = DomainFactory.GetNextIds();
            var dto = new DatabaseDto
            {
                NextIds = new int[] { nextBA, nextCat, nextOp },
                BankAccounts = new List<BankAccountDto>(),
                Categories = new List<CategoryDto>(),
                Operations = new List<OperationDto>()
            };

            // Заполняем
            foreach (var ba in _bankAccounts.Values)
            {
                dto.BankAccounts.Add(new BankAccountDto
                {
                    Id = ba.Id,
                    Name = ba.Name,
                    Balance = ba.Balance
                });
            }

            foreach (var c in _categories.Values)
            {
                dto.Categories.Add(new CategoryDto
                {
                    Id = c.Id,
                    Type = c.Type,
                    Name = c.Name
                });
            }

            foreach (var o in _operations.Values)
            {
                dto.Operations.Add(new OperationDto
                {
                    Id = o.Id,
                    Type = o.Type,
                    BankAccountId = o.BankAccountId,
                    Amount = o.Amount,
                    Date = o.Date.ToString("yyyy-MM-dd"),
                    CategoryId = o.CategoryId,
                    Description = o.Description
                });
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(dto, options);
            File.WriteAllText(_dbPath, json);
        }

        // DTO-классы, только для (де)сериализации
        private class DatabaseDto
        {
            public int[] NextIds { get; set; }
            public List<BankAccountDto> BankAccounts { get; set; }
            public List<CategoryDto> Categories { get; set; }
            public List<OperationDto> Operations { get; set; }
        }

        private class BankAccountDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Balance { get; set; }
        }

        private class CategoryDto
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
        }

        private class OperationDto
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public int BankAccountId { get; set; }
            public double Amount { get; set; }
            public string Date { get; set; }
            public int CategoryId { get; set; }
            public string Description { get; set; }
        }

        // ==============================
        // Методы CRUD
        // ==============================

        // BankAccount
        public void AddBankAccount(BankAccount account)
        {
            _bankAccounts[account.Id] = account;
            SaveToFile();
        }

        public BankAccount GetBankAccount(int id)
        {
            return _bankAccounts.ContainsKey(id) ? _bankAccounts[id] : null;
        }

        public List<BankAccount> GetAllBankAccounts()
        {
            return new List<BankAccount>(_bankAccounts.Values);
        }

        public void UpdateBankAccount(BankAccount account)
        {
            _bankAccounts[account.Id] = account;
            SaveToFile();
        }

        public void DeleteBankAccount(int id)
        {
            if (_bankAccounts.ContainsKey(id))
            {
                // Удалим операции, связанные с этим счётом
                var opsToRemove = new List<int>();
                foreach (var kv in _operations)
                {
                    if (kv.Value.BankAccountId == id)
                    {
                        opsToRemove.Add(kv.Key);
                    }
                }

                foreach (var opId in opsToRemove)
                {
                    _operations.Remove(opId);
                }

                _bankAccounts.Remove(id);
                SaveToFile();
            }
        }

        // Category
        public void AddCategory(Category category)
        {
            _categories[category.Id] = category;
            SaveToFile();
        }

        public Category GetCategory(int id)
        {
            return _categories.ContainsKey(id) ? _categories[id] : null;
        }

        public List<Category> GetAllCategories()
        {
            return new List<Category>(_categories.Values);
        }

        public void UpdateCategory(Category category)
        {
            _categories[category.Id] = category;
            SaveToFile();
        }

        public void DeleteCategory(int id)
        {
            if (_categories.ContainsKey(id))
            {
                // Удалим операции, связанные с категорией
                var opsToRemove = new List<int>();
                foreach (var kv in _operations)
                {
                    if (kv.Value.CategoryId == id)
                    {
                        opsToRemove.Add(kv.Key);
                    }
                }

                foreach (var opId in opsToRemove)
                {
                    _operations.Remove(opId);
                }

                _categories.Remove(id);
                SaveToFile();
            }
        }

        // Operation
        public void AddOperation(Operation op)
        {
            // Обновим баланс счёта
            var account = GetBankAccount(op.BankAccountId);
            if (account == null)
            {
                throw new Exception("Счёт не найден!");
            }

            if (op.Type == "income")
            {
                account.UpdateBalance(op.Amount);
            }
            else
            {
                if (account.Balance < op.Amount)
                {
                    throw new Exception("Недостаточно средств на счете. Баланс не может уйти в минус!");
                }
                account.UpdateBalance(-op.Amount);
            }

            _bankAccounts[account.Id] = account;
            _operations[op.Id] = op;
            SaveToFile();
        }

        public Operation GetOperation(int id)
        {
            return _operations.ContainsKey(id) ? _operations[id] : null;
        }

        public List<Operation> GetAllOperations()
        {
            return new List<Operation>(_operations.Values);
        }

        public void UpdateOperation(Operation op)
        {
            _operations[op.Id] = op;
            SaveToFile();
        }

        public void DeleteOperation(int id)
        {
            if (_operations.ContainsKey(id))
            {
                var op = _operations[id];
                // "Откат" баланса
                var account = GetBankAccount(op.BankAccountId);
                if (account != null)
                {
                    if (op.Type == "income")
                        account.UpdateBalance(-op.Amount);
                    else
                        account.UpdateBalance(op.Amount);

                    _bankAccounts[account.Id] = account;
                }

                _operations.Remove(id);
                SaveToFile();
            }
        }

        public void RecalculateBalances()
        {
            // Обнуляем все счета
            foreach (var ba in _bankAccounts.Values)
            {
                ba.UpdateBalance(-ba.Balance); // привели к 0
            }

            // Заново проходим операции
            foreach (var op in _operations.Values)
            {
                var acc = _bankAccounts[op.BankAccountId];
                if (op.Type == "income")
                    acc.UpdateBalance(op.Amount);
                else
                    acc.UpdateBalance(-op.Amount);
            }

            SaveToFile();
        }
    }
}