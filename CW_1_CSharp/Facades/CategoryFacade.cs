using System;
using System.Collections.Generic;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;

namespace CW_1_CSharp.Facades
{
    public class CategoryFacade
    {
        private readonly DataProxy _proxy;

        public CategoryFacade(DataProxy proxy)
        {
            _proxy = proxy;
        }

        public void CreateCategory(string type, string name)
        {
            var cat = DomainFactory.CreateCategory(type, name);
            _proxy.AddCategory(cat);
        }

        public void UpdateCategoryName(int categoryId, string newName)
        {
            var cat = _proxy.GetCategory(categoryId);
            if (cat == null)
            {
                throw new Exception("Категория не найдена.");
            }
            cat.Name = newName;
            _proxy.UpdateCategory(cat);
        }

        public void DeleteCategory(int categoryId)
        {
            _proxy.DeleteCategory(categoryId);
        }

        public List<Category> ListCategories()
        {
            return _proxy.GetAllCategories();
        }
    }
}