using System;
using System.Globalization;
using CW_1_CSharp.Proxies;
using CW_1_CSharp.Facades;
using CW_1_CSharp.Commands;
using CW_1_CSharp.ImportExport;

namespace CW_1_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Инициализируем "прокси", который хранит данные и синхронизирует их с JSON-файлом
            var dataProxy = new DataProxy("database.json");

            // Фасады для работы с сущностями (счет, категория, операция, аналитика)
            var accFacade = new BankAccountFacade(dataProxy);
            var catFacade = new CategoryFacade(dataProxy);
            var opFacade = new OperationFacade(dataProxy);
            var anFacade = new AnalyticsFacade(dataProxy);

            while (true)
            {
                PrintMenu();
                Console.Write("Выберите действие: ");
                var choice = Console.ReadLine()?.Trim();

                if (choice == "0")
                {
                    Console.WriteLine("Выход...");
                    break;
                }
                else if (choice == "1")
                {
                    MenuBankAccounts(accFacade);
                }
                else if (choice == "2")
                {
                    MenuCategories(catFacade);
                }
                else if (choice == "3")
                {
                    MenuOperations(opFacade);
                }
                else if (choice == "4")
                {
                    MenuAnalytics(anFacade);
                }
                else if (choice == "5")
                {
                    MenuImport(dataProxy);
                }
                else if (choice == "6")
                {
                    MenuExport(dataProxy);
                }
                else if (choice == "7")
                {
                    dataProxy.RecalculateBalances();
                    Console.WriteLine("Балансы пересчитаны!");
                }
                else
                {
                    Console.WriteLine("Неизвестный выбор.");
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("\n===== Главное меню =====");
            Console.WriteLine("1. Управление счетами (create/update/delete/list)");
            Console.WriteLine("2. Управление категориями (create/update/delete/list)");
            Console.WriteLine("3. Управление операциями (create/delete/list)");
            Console.WriteLine("4. Аналитика (разница доход-расход за период, группировка)");
            Console.WriteLine("5. Импорт данных (CSV, JSON, YAML)");
            Console.WriteLine("6. Экспорт данных (CSV, JSON, YAML)");
            Console.WriteLine("7. Пересчитать балансы (recalculate)");
            Console.WriteLine("0. Выход");
        }

        static void MenuBankAccounts(BankAccountFacade accFacade)
        {
            Console.WriteLine(" a) Создать счет");
            Console.WriteLine(" b) Переименовать счет");
            Console.WriteLine(" c) Удалить счет");
            Console.WriteLine(" d) Показать все счета");
            Console.Write("Выберите: ");
            var sub = Console.ReadLine()?.Trim().ToLower();
            switch (sub)
            {
                case "a":
                    Console.Write("Введите название счета: ");
                    var name = Console.ReadLine();
                    Console.Write("Введите начальный баланс: ");
                    var balanceStr = Console.ReadLine();
                    if (double.TryParse(balanceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double balance))
                    {
                        var cmd = new CreateAccountCommand(accFacade, name, balance);
                        var decoratedCmd = new TimeMeasurementDecorator(cmd);
                        decoratedCmd.Execute();
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод баланса!");
                    }
                    break;
                case "b":
                    Console.Write("Введите ID счета: ");
                    if (int.TryParse(Console.ReadLine(), out int accIdForUpdate))
                    {
                        Console.Write("Введите новое имя счета: ");
                        var newName = Console.ReadLine();
                        try
                        {
                            var cmd = new UpdateAccountNameCommand(accFacade, accIdForUpdate, newName);
                            var measuredCmd = new TimeMeasurementDecorator(cmd);
                            measuredCmd.Execute();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID!");
                    }
                    break;
                case "c":
                    Console.Write("Введите ID счета: ");
                    if (int.TryParse(Console.ReadLine(), out int accIdForDelete))
                    {
                        var cmd = new DeleteAccountCommand(accFacade, accIdForDelete);
                        cmd.Execute();
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID!");
                    }
                    break;
                case "d":
                    var accounts = accFacade.ListAccounts();
                    Console.WriteLine("Счета:");
                    foreach (var a in accounts)
                    {
                        Console.WriteLine(a);
                    }
                    break;
            }
        }

        static void MenuCategories(CategoryFacade catFacade)
        {
            Console.WriteLine(" a) Создать категорию");
            Console.WriteLine(" b) Переименовать категорию");
            Console.WriteLine(" c) Удалить категорию");
            Console.WriteLine(" d) Показать все категории");
            Console.Write("Выберите: ");
            var sub = Console.ReadLine()?.Trim().ToLower();
            switch (sub)
            {
                case "a":
                    Console.Write("Тип категории (income/expense): ");
                    var catType = Console.ReadLine();
                    Console.Write("Название категории: ");
                    var catName = Console.ReadLine();
                    try
                    {
                        var cmd = new CreateCategoryCommand(catFacade, catType, catName);
                        var measuredCmd = new TimeMeasurementDecorator(cmd);
                        measuredCmd.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка: " + ex.Message);
                    }
                    break;
                case "b":
                    Console.Write("Введите ID категории: ");
                    if (int.TryParse(Console.ReadLine(), out int catIdForUpdate))
                    {
                        Console.Write("Введите новое имя категории: ");
                        var newCatName = Console.ReadLine();
                        try
                        {
                            var cmd = new UpdateCategoryNameCommand(catFacade, catIdForUpdate, newCatName);
                            cmd.Execute();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID!");
                    }
                    break;
                case "c":
                    Console.Write("Введите ID категории: ");
                    if (int.TryParse(Console.ReadLine(), out int catIdForDelete))
                    {
                        var cmd = new DeleteCategoryCommand(catFacade, catIdForDelete);
                        cmd.Execute();
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID!");
                    }
                    break;
                case "d":
                    var cats = catFacade.ListCategories();
                    Console.WriteLine("Категории:");
                    foreach (var c in cats)
                    {
                        Console.WriteLine(c);
                    }
                    break;
            }
        }

        static void MenuOperations(OperationFacade opFacade)
        {
            Console.WriteLine(" a) Создать операцию (тип определяется категорией, дата = сегодня)");
            Console.WriteLine(" b) Удалить операцию");
            Console.WriteLine(" c) Показать все операции");
            Console.Write("Выберите: ");
            var sub = Console.ReadLine()?.Trim().ToLower();
            switch (sub)
            {
                case "a":
                    Console.Write("Введите ID счета: ");
                    var accIdStr = Console.ReadLine();
                    Console.Write("Введите ID категории: ");
                    var catIdStr = Console.ReadLine();
                    Console.Write("Введите сумму: ");
                    var amountStr = Console.ReadLine();
                    // Устанавливаем дату как сегодняшнюю:
                    var dateStr = DateTime.Today.ToString("yyyy-MM-dd");
                    Console.WriteLine($"Дата операции автоматически установлена: {dateStr}");
                    Console.Write("Введите описание (необязательно): ");
                    var descr = Console.ReadLine();

                    try
                    {
                        int accId = int.Parse(accIdStr);
                        int catId = int.Parse(catIdStr);
                        double amount = double.Parse(amountStr, CultureInfo.InvariantCulture);

                        opFacade.CreateOperation(
                            accountId: accId,
                            amount: amount,
                            dateStr: dateStr,
                            categoryId: catId,
                            description: descr
                        );
                        Console.WriteLine("Операция успешно создана (тип определяется категорией).");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка: " + ex.Message);
                    }
                    break;
                case "b":
                    Console.Write("Введите ID операции: ");
                    if (int.TryParse(Console.ReadLine(), out int opId))
                    {
                        opFacade.DeleteOperation(opId);
                        Console.WriteLine("Операция удалена.");
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID!");
                    }
                    break;
                case "c":
                    var ops = opFacade.ListOperations();
                    Console.WriteLine("Операции:");
                    foreach (var o in ops)
                    {
                        Console.WriteLine(o);
                    }
                    break;
            }
        }

        static void MenuAnalytics(AnalyticsFacade anFacade)
        {
            Console.WriteLine(" a) Разница доходов и расходов за период");
            Console.WriteLine(" b) Группировка по категориям за период");
            Console.Write("Выберите: ");
            var sub = Console.ReadLine()?.Trim().ToLower();
            switch (sub)
            {
                case "a":
                    Console.Write("Введите стартовую дату (yyyy-MM-dd): ");
                    var startA = Console.ReadLine();
                    Console.Write("Введите конечную дату (yyyy-MM-dd): ");
                    var endA = Console.ReadLine();
                    var diff = anFacade.GetIncomeExpenseDiff(startA, endA);
                    Console.WriteLine($"Разница (доход - расход) = {diff}");
                    break;
                case "b":
                    Console.Write("Введите стартовую дату (yyyy-MM-dd): ");
                    var startB = Console.ReadLine();
                    Console.Write("Введите конечную дату (yyyy-MM-dd): ");
                    var endB = Console.ReadLine();
                    var grouped = anFacade.GroupByCategory(startB, endB);
                    Console.WriteLine("Группировка по категориям (сумма операций):");
                    foreach (var kv in grouped)
                    {
                        Console.WriteLine($"Category #{kv.Key} | {kv.Value.catName}({kv.Value.catType}) => {kv.Value.total}");
                    }
                    break;
            }
        }

        static void MenuImport(DataProxy dataProxy)
        {
            Console.WriteLine("Импорт:");
            Console.WriteLine(" a) CSV");
            Console.WriteLine(" b) JSON");
            Console.WriteLine(" c) YAML");
            Console.Write("Выберите: ");
            var sub = Console.ReadLine()?.Trim().ToLower();

            Console.Write("Укажите путь к файлу: ");
            var path = Console.ReadLine();

            if (!System.IO.File.Exists(path))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }

            DataImporter importer = null;
            switch (sub)
            {
                case "a":
                    importer = new CsvImporter();
                    break;
                case "b":
                    importer = new JsonImporter();
                    break;
                case "c":
                    importer = new YamlImporter();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор формата.");
                    return;
            }

            try
            {
                importer.ImportData(path, dataProxy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка импорта: " + ex.Message);
            }
        }

        static void MenuExport(DataProxy dataProxy)
        {
            Console.WriteLine("Экспорт:");
            Console.WriteLine(" a) CSV");
            Console.WriteLine(" b) JSON");
            Console.WriteLine(" c) YAML");
            Console.Write("Выберите: ");
            var sub = Console.ReadLine()?.Trim().ToLower();

            Console.Write("Укажите путь для сохранения файла: ");
            var path = Console.ReadLine();

            IDataVisitor visitor = null;
            switch (sub)
            {
                case "a":
                    visitor = new CsvVisitor();
                    break;
                case "b":
                    visitor = new JsonVisitor();
                    break;
                case "c":
                    visitor = new YamlVisitor();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор формата.");
                    return;
            }

            var bas = dataProxy.GetAllBankAccounts();
            var cats = dataProxy.GetAllCategories();
            var ops = dataProxy.GetAllOperations();

            foreach (var b in bas) visitor.VisitBankAccount(b);
            foreach (var c in cats) visitor.VisitCategory(c);
            foreach (var o in ops) visitor.VisitOperation(o);

            var result = visitor.GetResult();
            try
            {
                System.IO.File.WriteAllText(path, result);
                Console.WriteLine("Экспорт выполнен успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка экспорта: " + ex.Message);
            }
        }
    }
}
