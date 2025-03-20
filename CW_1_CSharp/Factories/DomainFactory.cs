using System;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.Factories
{
    public static class DomainFactory
    {
        private static int _nextBankAccountId = 1;
        private static int _nextCategoryId = 1;
        private static int _nextOperationId = 1;

        public static BankAccount CreateBankAccount(string name, double balance = 0.0)
        {
            if (balance < 0)
                throw new ArgumentException("Начальный баланс не может быть отрицательным.");

            var ba = new BankAccount(_nextBankAccountId, name, balance);
            _nextBankAccountId++;
            return ba;
        }

        public static Category CreateCategory(string type, string name)
        {
            var cat = new Category(_nextCategoryId, type, name);
            _nextCategoryId++;
            return cat;
        }

        public static Operation CreateOperation(int bankAccountId,
            double amount,
            DateTime date,
            int categoryId,
            string description,
            string categoryType)
        {
            var op = new Operation(_nextOperationId,
                bankAccountId,
                amount,
                date,
                categoryId,
                description,
                categoryType);
            _nextOperationId++;
            return op;
        }

        public static void SetNextIds(int nextBankAccountId, int nextCategoryId, int nextOperationId)
        {
            _nextBankAccountId = nextBankAccountId;
            _nextCategoryId = nextCategoryId;
            _nextOperationId = nextOperationId;
        }

        public static (int, int, int) GetNextIds()
        {
            return (_nextBankAccountId, _nextCategoryId, _nextOperationId);
        }
    }
}