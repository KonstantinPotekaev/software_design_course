using System;
using System.Globalization;
using System.Collections.Generic;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.Facades
{
    public class AnalyticsFacade
    {
        private readonly DataProxy _proxy;

        public AnalyticsFacade(DataProxy proxy)
        {
            _proxy = proxy;
        }

        /// <summary>
        /// Разница доходов и расходов за период.
        /// Формат дат: yyyy-MM-dd
        /// </summary>
        public double GetIncomeExpenseDiff(string startDateStr, string endDateStr)
        {
            var start = DateTime.ParseExact(startDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(endDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var allOps = _proxy.GetAllOperations();
            double totalIncome = 0;
            double totalExpense = 0;

            foreach (var op in allOps)
            {
                if (op.Date >= start && op.Date <= end)
                {
                    if (op.Type == "income")
                        totalIncome += op.Amount;
                    else
                        totalExpense += op.Amount;
                }
            }
            return totalIncome - totalExpense;
        }

        /// <summary>
        /// Группировка сумм операций по категориям за период.
        /// Возвращает словарь: 
        ///   categoryId -> (CategoryName, CategoryType, сумма)
        /// </summary>
        public Dictionary<int, (string catName, string catType, double total)> GroupByCategory(string startDateStr, string endDateStr)
        {
            var start = DateTime.ParseExact(startDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(endDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var allOps = _proxy.GetAllOperations();
            var cats = _proxy.GetAllCategories();
            var catMap = new Dictionary<int, Category>();
            foreach (var c in cats)
            {
                catMap[c.Id] = c;
            }

            var result = new Dictionary<int, (string catName, string catType, double total)>();

            foreach (var op in allOps)
            {
                if (op.Date >= start && op.Date <= end)
                {
                    var cat = catMap[op.CategoryId];
                    if (!result.ContainsKey(cat.Id))
                    {
                        result[cat.Id] = (cat.Name, cat.Type, 0.0);
                    }

                    double current = result[cat.Id].total;
                    current += op.Amount; // для расхода будет += (положительное число), но мы понимаем, что это расход
                    result[cat.Id] = (cat.Name, cat.Type, current);
                }
            }

            return result;
        }
    }
}
