using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class CreateCategoryCommand : ICommand
    {
        private readonly CategoryFacade _facade;
        private readonly string _type; // "income" или "expense"
        private readonly string _name;

        public CreateCategoryCommand(CategoryFacade facade, string type, string name)
        {
            _facade = facade;
            _type = type;
            _name = name;
        }

        public void Execute()
        {
            _facade.CreateCategory(_type, _name);
            Console.WriteLine("Категория успешно создана.");
        }
    }
}