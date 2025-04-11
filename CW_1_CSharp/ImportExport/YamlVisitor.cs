using System.Collections.Generic;
using CW_1_CSharp.Domain;
using YamlDotNet.Serialization;

namespace CW_1_CSharp.ImportExport
{
    public class YamlVisitor : IDataVisitor
    {
        private List<object> _bankAccounts;
        private List<object> _categories;
        private List<object> _operations;

        public YamlVisitor()
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
            var data = new {
                bank_accounts = _bankAccounts,
                categories = _categories,
                operations = _operations
            };

            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(data);
        }
    }
}