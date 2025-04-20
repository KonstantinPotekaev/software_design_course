using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class DeleteCategoryCommand : ICommand
    {
        private readonly CategoryFacade _facade;
        private readonly int _categoryId;

        public DeleteCategoryCommand(CategoryFacade facade, int categoryId)
        {
            _facade = facade;
            _categoryId = categoryId;
        }

        public void Execute()
        {
            _facade.DeleteCategory(_categoryId);
            Console.WriteLine("Категория удалена.");
        }
    }
}