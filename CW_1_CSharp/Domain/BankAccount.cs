using System;

namespace CW_1_CSharp.Domain
{
    public class BankAccount
    {
        private int _id;
        private string _name;
        private double _balance;

        public BankAccount(int id, string name, double balance)
        {
            _id = id;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название счёта не может быть пустым!");
            _name = name;
            _balance = balance;
        }

        public int Id => _id;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название счёта не может быть пустым!");
                _name = value;
            }
        }

        public double Balance => _balance;

        public void UpdateBalance(double amount)
        {
            _balance += amount;
        }

        public override string ToString()
        {
            return $"[BankAccount #{_id}] {_name} (balance: {_balance})";
        }
    }
}