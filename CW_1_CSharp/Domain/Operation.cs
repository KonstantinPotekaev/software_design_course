using System;

namespace CW_1_CSharp.Domain
{
    public class Operation
    {
        private int _id;
        private string _type;
        private int _bankAccountId;
        private double _amount;
        private DateTime _date;
        private int _categoryId;
        private string _description;

        public Operation(int id,
            int bankAccountId,
            double amount,
            DateTime date,
            int categoryId,
            string description,
            string categoryType)
        {
            if (categoryType != "income" && categoryType != "expense")
            {
                throw new ArgumentException("Тип категории должен быть 'income' или 'expense'.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Сумма операции должна быть > 0.");
            }

            _id = id;
            _type = categoryType;
            _bankAccountId = bankAccountId;
            _amount = amount;
            _date = date;
            _categoryId = categoryId;
            _description = description;
        }

        public int Id => _id;
        public string Type => _type;
        public int BankAccountId => _bankAccountId;
        public double Amount => _amount;
        public DateTime Date => _date;
        public int CategoryId => _categoryId;

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public override string ToString()
        {
            return $"[Operation #{_id}] {_type} | amount: {_amount} | date: {_date:yyyy-MM-dd} | " +
                   $"category: {_categoryId} | account: {_bankAccountId} | desc: {_description}";
        }
    }
}