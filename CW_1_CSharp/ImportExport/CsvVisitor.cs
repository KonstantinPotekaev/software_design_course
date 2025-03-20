using System.IO;
using System.Text;
using System.Collections.Generic;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.ImportExport
{
    public class CsvVisitor : IDataVisitor
    {
        private List<string> _bankAccountRows;
        private List<string> _categoryRows;
        private List<string> _operationRows;

        public CsvVisitor()
        {
            _bankAccountRows = new List<string>();
            _categoryRows = new List<string>();
            _operationRows = new List<string>();
        }

        public void VisitBankAccount(BankAccount bankAccount)
        {
            // "id,name,balance"
            _bankAccountRows.Add($"{bankAccount.Id},{bankAccount.Name},{bankAccount.Balance}");
        }

        public void VisitCategory(Category category)
        {
            // "id,type,name"
            _categoryRows.Add($"{category.Id},{category.Type},{category.Name}");
        }

        public void VisitOperation(Operation operation)
        {
            // "id,type,bank_account_id,amount,date,category_id,description"
            _operationRows.Add($"{operation.Id},{operation.Type},{operation.BankAccountId},{operation.Amount},{operation.Date:yyyy-MM-dd},{operation.CategoryId},{operation.Description}");
        }

        public string GetResult()
        {
            var sb = new StringBuilder();

            if (_bankAccountRows.Count > 0)
            {
                sb.AppendLine("id,name,balance");
                foreach (var row in _bankAccountRows)
                {
                    sb.AppendLine(row);
                }
                sb.AppendLine();
            }

            if (_categoryRows.Count > 0)
            {
                sb.AppendLine("id,type,name");
                foreach (var row in _categoryRows)
                {
                    sb.AppendLine(row);
                }
                sb.AppendLine();
            }

            if (_operationRows.Count > 0)
            {
                sb.AppendLine("id,type,bank_account_id,amount,date,category_id,description");
                foreach (var row in _operationRows)
                {
                    sb.AppendLine(row);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
