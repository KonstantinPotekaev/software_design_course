using System;

namespace CW_1_CSharp.Domain
{
    public class Category
    {
        private int _id;
        private string _type; // "income" или "expense"
        private string _name;

        public Category(int id, string type, string name)
        {
            if (type != "income" && type != "expense")
                throw new ArgumentException("Тип категории должен быть 'income' или 'expense'.");

            _id = id;
            _type = type;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название категории не может быть пустым!");
            _name = name;
        }

        public int Id => _id;
        public string Type => _type;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название категории не может быть пустым!");
                _name = value;
            }
        }

        public override string ToString()
        {
            return $"[Category #{_id}] {_name} ({_type})";
        }
    }
}