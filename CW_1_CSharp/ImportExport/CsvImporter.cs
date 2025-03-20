using System;
using System.Collections.Generic;
using System.Globalization;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Domain;

namespace CW_1_CSharp.ImportExport
{
    public class CsvImporter : DataImporter
    {
        protected override object ParseData(string rawData)
        {
            var lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 1)
                return new List<Dictionary<string, string>>();

            var header = lines[0].Split(',');
            var result = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var row = lines[i].Split(',');
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (int c = 0; c < header.Length; c++)
                {
                    var key = header[c].Trim();
                    var val = row.Length > c ? row[c].Trim() : "";
                    dict[key] = val;
                }
                result.Add(dict);
            }
            return result;
        }

        protected override void StoreData(object data, DataProxy dataProxy)
        {
            var list = data as List<Dictionary<string, string>>;
            if (list == null)
                return;

            foreach (var row in list)
            {
                // Если строка содержит ключ bank_account_id – импортируем операцию
                if (row.ContainsKey("bank_account_id"))
                {
                    try
                    {
                        int accId = int.Parse(row["bank_account_id"]);
                        double amount = double.Parse(row["amount"], CultureInfo.InvariantCulture);
                        string dateStr = row["date"];
                        int categoryId = int.Parse(row["category_id"]);
                        string desc = row.ContainsKey("description") ? row["description"] : "";
                        var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var category = dataProxy.GetCategory(categoryId);
                        if (category == null)
                        {
                            Console.WriteLine($"Категория с ID {categoryId} не найдена для операции.");
                            continue;
                        }
                        // Создаем операцию: тип определяется из категории
                        var op = DomainFactory.CreateOperation(accId, amount, date, categoryId, desc, category.Type);
                        dataProxy.AddOperation(op);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка импорта операции: " + ex.Message);
                    }
                }
                // Если строка содержит ключ balance – импортируем счет
                else if (row.ContainsKey("balance"))
                {
                    try
                    {
                        int id = int.Parse(row["id"]);
                        string name = row["name"];
                        double balance = double.Parse(row["balance"], CultureInfo.InvariantCulture);
                        // Создаем счет с сохранением id
                        var account = new BankAccount(id, name, balance);
                        dataProxy.AddBankAccount(account);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка импорта счета: " + ex.Message);
                    }
                }
                // Если строка содержит ключ type, но нет balance – импортируем категорию
                else if (row.ContainsKey("type"))
                {
                    try
                    {
                        int id = int.Parse(row["id"]);
                        string type = row["type"];
                        string name = row["name"];
                        var category = new Category(id, type, name);
                        dataProxy.AddCategory(category);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка импорта категории: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестный формат строки CSV.");
                }
            }

            Console.WriteLine("CSV-данные импортированы успешно!");
        }
    }
}
