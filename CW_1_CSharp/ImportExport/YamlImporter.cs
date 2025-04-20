using System;
using System.Collections.Generic;
using System.Globalization;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Domain;
using YamlDotNet.Serialization;

namespace CW_1_CSharp.ImportExport
{
    public class YamlImporter : DataImporter
    {
        protected override object ParseData(string rawData)
        {
            var deserializer = new DeserializerBuilder().Build();
            var data = deserializer.Deserialize<Dictionary<string, object>>(rawData);
            return data;
        }

        protected override void StoreData(object data, DataProxy dataProxy)
        {
            var dict = data as Dictionary<string, object>;
            if (dict == null)
            {
                Console.WriteLine("Неверный формат YAML!");
                return;
            }

            // ----- BankAccounts -----
            if (dict.ContainsKey("bank_accounts"))
            {
                var bankAccountsObj = dict["bank_accounts"] as List<object>;
                if (bankAccountsObj != null)
                {
                    foreach (var item in bankAccountsObj)
                    {
                        var itemDict = item as Dictionary<object, object>;
                        if (itemDict != null)
                        {
                            int id = Convert.ToInt32(itemDict["id"]);
                            string name = itemDict["name"].ToString();
                            double balance = Convert.ToDouble(itemDict["balance"], CultureInfo.InvariantCulture);

                            var account = new BankAccount(id, name, balance);
                            dataProxy.AddBankAccount(account);
                        }
                    }
                }
            }

            // ----- Categories -----
            if (dict.ContainsKey("categories"))
            {
                var categoriesObj = dict["categories"] as List<object>;
                if (categoriesObj != null)
                {
                    foreach (var item in categoriesObj)
                    {
                        var itemDict = item as Dictionary<object, object>;
                        if (itemDict != null)
                        {
                            int id = Convert.ToInt32(itemDict["id"]);
                            string type = itemDict["type"].ToString();
                            string name = itemDict["name"].ToString();

                            var category = new Category(id, type, name);
                            dataProxy.AddCategory(category);
                        }
                    }
                }
            }

            // ----- Operations -----
            if (dict.ContainsKey("operations"))
            {
                var operationsObj = dict["operations"] as List<object>;
                if (operationsObj != null)
                {
                    foreach (var item in operationsObj)
                    {
                        var itemDict = item as Dictionary<object, object>;
                        if (itemDict != null)
                        {
                            int id = Convert.ToInt32(itemDict["id"]);
                            // Поле "type" из файла игнорируем (оно определяется по категории)
                            int bankAccountId = Convert.ToInt32(itemDict["bank_account_id"]);
                            double amount = Convert.ToDouble(itemDict["amount"], CultureInfo.InvariantCulture);
                            string dateStr = itemDict["date"].ToString();
                            int categoryId = Convert.ToInt32(itemDict["category_id"]);
                            string desc = "";
                            if (itemDict.ContainsKey("description") && itemDict["description"] != null)
                            {
                                desc = itemDict["description"].ToString();
                            }

                            var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var category = dataProxy.GetCategory(categoryId);
                            if (category == null)
                            {
                                Console.WriteLine($"Категория с ID {categoryId} не найдена для операции ID {id}.");
                                continue;
                            }

                            var op = DomainFactory.CreateOperation(bankAccountId, amount, date, categoryId, desc,
                                category.Type);
                            dataProxy.AddOperation(op);
                        }
                    }
                }
            }

            Console.WriteLine("YAML-данные импортированы успешно!");
        }
    }
}