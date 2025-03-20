using System;
using System.Globalization;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.Facades
{
    public class OperationFacade
    {
        private readonly DataProxy _proxy;

        public OperationFacade(DataProxy proxy)
        {
            _proxy = proxy;
        }

        public void CreateOperation(int accountId,
            double amount,
            string dateStr,
            int categoryId,
            string description)
        {
            var category = _proxy.GetCategory(categoryId);
            if (category == null)
            {
                throw new Exception("Указанная категория не найдена!");
            }

            var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var op = DomainFactory.CreateOperation(
                accountId,
                amount,
                date,
                categoryId,
                description,
                category.Type
            );


            _proxy.AddOperation(op);
        }

        public void DeleteOperation(int operationId)
        {
            _proxy.DeleteOperation(operationId);
        }

        public System.Collections.Generic.List<Operation> ListOperations()
        {
            return _proxy.GetAllOperations();
        }
    }
}