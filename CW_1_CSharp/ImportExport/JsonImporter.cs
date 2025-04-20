using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Globalization;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.ImportExport
{
    public class JsonImporter : DataImporter
    {
        protected override object ParseData(string rawData)
        {
            var topLevelList = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(rawData);
            return topLevelList;
        }

        protected override void StoreData(object data, DataProxy dataProxy)
        {
            var topLevelList = data as List<Dictionary<string, JsonElement>>;
            if (topLevelList == null || topLevelList.Count == 0)
            {
                Console.WriteLine("JSON пустой или не соответствует ожидаемому формату!");
                return;
            }

            var mainDict = topLevelList[0];

            // ----- BankAccounts -----
            if (mainDict.ContainsKey("bank_accounts"))
            {
                var bankAccountsElement = mainDict["bank_accounts"];
                var bankAccountsList =
                    JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(bankAccountsElement.GetRawText());
                if (bankAccountsList != null)
                {
                    foreach (var baObj in bankAccountsList)
                    {
                        int id = baObj["id"].GetInt32();
                        string name = baObj["name"].GetString();
                        double balance = baObj["balance"].GetDouble();
                        // Создаём объект с сохранением id (вручную)
                        var account = new BankAccount(id, name, balance);
                        dataProxy.AddBankAccount(account);
                    }
                }
            }

            // ----- Categories -----
            if (mainDict.ContainsKey("categories"))
            {
                var categoriesElement = mainDict["categories"];
                var categoriesList =
                    JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(categoriesElement.GetRawText());
                if (categoriesList != null)
                {
                    foreach (var catObj in categoriesList)
                    {
                        int id = catObj["id"].GetInt32();
                        string type = catObj["type"].GetString();
                        string name = catObj["name"].GetString();
                        var category = new Category(id, type, name);
                        dataProxy.AddCategory(category);
                    }
                }
            }

            // ----- Operations -----
            if (mainDict.ContainsKey("operations"))
            {
                var operationsElement = mainDict["operations"];
                var operationsList =
                    JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(operationsElement.GetRawText());
                if (operationsList != null)
                {
                    foreach (var opObj in operationsList)
                    {
                        int id = opObj["id"].GetInt32();
                        // Игнорируем поле "type" из JSON, т.к. тип определяется по категории
                        int bankAccountId = opObj["bank_account_id"].GetInt32();
                        double amount = opObj["amount"].GetDouble();
                        string dateStr = opObj["date"].GetString();
                        int categoryId = opObj["category_id"].GetInt32();
                        string desc = "";
                        if (opObj.ContainsKey("description") && opObj["description"].ValueKind != JsonValueKind.Null)
                            desc = opObj["description"].GetString();

                        var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        // Получаем категорию по categoryId
                        var category = dataProxy.GetCategory(categoryId);
                        if (category == null)
                        {
                            Console.WriteLine($"Категория с ID {categoryId} не найдена для операции ID {id}.");
                            continue;
                        }

                        // Создаём операцию, передавая category.Type
                        var op = new Operation(id, bankAccountId, amount, date, categoryId, desc, category.Type);
                        dataProxy.AddOperation(op);
                    }
                }
            }

            Console.WriteLine("JSON-данные импортированы успешно!");
        }
    }
}