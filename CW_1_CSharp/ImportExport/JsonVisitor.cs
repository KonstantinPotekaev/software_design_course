using System.Collections.Generic;
using System.Text.Json;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.ImportExport
{
    public class JsonVisitor : IDataVisitor
    {
        private List<object> _bankAccounts;
        private List<object> _categories;
        private List<object> _operations;

        public JsonVisitor()
        {
            _bankAccounts = new List<object>();
            _categories = new List<object>();
            _operations = new List<object>();
        }

        public void VisitBankAccount(BankAccount bankAccount)
        {
            _bankAccounts.Add(new {
                id = bankAccount.Id,
                name = bankAccount.Name,
                balance = bankAccount.Balance
            });
        }

        public void VisitCategory(Category category)
        {
            _categories.Add(new {
                id = category.Id,
                type = category.Type,
                name = category.Name
            });
        }

        public void VisitOperation(Operation operation)
        {
            _operations.Add(new {
                id = operation.Id,
                type = operation.Type,
                bank_account_id = operation.BankAccountId,
                amount = operation.Amount,
                date = operation.Date.ToString("yyyy-MM-dd"),
                category_id = operation.CategoryId,
                description = operation.Description
            });
        }

        public string GetResult()
        {
            var full = new {
                bank_accounts = _bankAccounts,
                categories = _categories,
                operations = _operations
            };
            
            var result = new[] { full };

            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}