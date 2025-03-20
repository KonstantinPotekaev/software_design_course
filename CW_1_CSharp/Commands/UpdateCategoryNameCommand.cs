using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class UpdateCategoryNameCommand : ICommand
    {
        private readonly CategoryFacade _facade;
        private readonly int _categoryId;
        private readonly string _newName;

        public UpdateCategoryNameCommand(CategoryFacade facade, int categoryId, string newName)
        {
            _facade = facade;
            _categoryId = categoryId;
            _newName = newName;
        }

        public void Execute()
        {
            _facade.UpdateCategoryName(_categoryId, _newName);
            Console.WriteLine("Название категории обновлено.");
        }
    }
}