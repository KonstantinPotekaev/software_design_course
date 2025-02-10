using System;
using System.Collections.Generic;
using System.Linq;
using MINI_HW_1.Domain;
using MINI_HW_1.Domain.Animals;
using MINI_HW_1.Domain.Things;
using MINI_HW_1.AnimalCreators;
using MINI_HW_1.ThingCreators;

namespace MINI_HW_1.UI
{
    public class MenuManager
    {
        private readonly Zoo _zoo;
        private readonly IEnumerable<IAnimalCreator> _animalCreators;
        private readonly IEnumerable<IThingCreator> _thingCreators;

        public MenuManager(Zoo zoo, IEnumerable<IAnimalCreator> animalCreators, IEnumerable<IThingCreator> thingCreators)
        {
            _zoo = zoo;
            _animalCreators = animalCreators;
            _thingCreators = thingCreators;
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("===== Меню Зоопарка =====");
                Console.WriteLine("1. Добавить животное");
                Console.WriteLine("2. Вывести общее количество кг еды для животных");
                Console.WriteLine("3. Вывести список животных для контактного зоопарка");
                Console.WriteLine("4. Вывести список всех инвентарных объектов");
                Console.WriteLine("5. Добавить инвентарную вещь");
                Console.WriteLine("6. Вывести список животных");
                Console.WriteLine("7. Вывести список вещей");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите опцию: ");
                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddAnimalMenu();
                        break;
                    case "2":
                        DisplayTotalFood();
                        break;
                    case "3":
                        DisplayContactZooCandidates();
                        break;
                    case "4":
                        DisplayInventoryItems();
                        break;
                    case "5":
                        AddThingMenu();
                        break;
                    case "6":
                        ListAnimals();
                        break;
                    case "7":
                        ListThings();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверная опция. Попробуйте снова.");
                        break;
                }
            }
            Console.WriteLine("Приложение завершено.");
        }

        private void AddAnimalMenu()
        {
            Console.WriteLine("Выберите тип животного для добавления:");
            int index = 1;
            foreach (var creator in _animalCreators)
            {
                Console.WriteLine($"{index}. {creator.AnimalTypeName}");
                index++;
            }
            int choice = ReadInt("Ваш выбор: ");
            if (choice < 1 || choice > _animalCreators.Count())
            {
                Console.WriteLine("Неверный выбор");
                return;
            }
            var selectedCreator = _animalCreators.ElementAt(choice - 1);
            Animal newAnimal = selectedCreator.CreateAnimal();
            bool success = _zoo.AddAnimal(newAnimal);
            Console.WriteLine(success
                ? $"Животное {newAnimal.Name} успешно добавлено."
                : $"Животное {newAnimal.Name} не прошло проверку и не добавлено.");
        }

        private void AddThingMenu()
        {
            Console.WriteLine("Выберите тип вещи для добавления:");
            int index = 1;
            foreach (var creator in _thingCreators)
            {
                Console.WriteLine($"{index}. {creator.ThingTypeName}");
                index++;
            }
            int choice = ReadInt("Ваш выбор: ");
            if (choice < 1 || choice > _thingCreators.Count())
            {
                Console.WriteLine("Неверный выбор");
                return;
            }
            var selectedCreator = _thingCreators.ElementAt(choice - 1);
            var newThing = selectedCreator.CreateThing();
            _zoo.AddThing(newThing);
            Console.WriteLine($"Вещь {newThing.Name} успешно добавлена.");
        }

        private void DisplayTotalFood()
        {
            int totalFood = _zoo.GetTotalFoodConsumption();
            Console.WriteLine($"Общее количество еды: {totalFood} кг/день.");
        }

        private void DisplayContactZooCandidates()
        {
            var candidates = _zoo.GetContactZooCandidates();
            if (candidates.Count == 0)
            {
                Console.WriteLine("Нет животных, подходящих для контактного зоопарка.");
            }
            else
            {
                Console.WriteLine("Животные для контактного зоопарка:");
                foreach (var animal in candidates)
                {
                    Console.WriteLine(animal.GetInfo());
                }
            }
        }

        private void DisplayInventoryItems()
        {
            var inventoryItems = _zoo.GetAllInventoryItems();
            if (inventoryItems.Count == 0)
            {
                Console.WriteLine("Нет инвентарных объектов.");
            }
            else
            {
                Console.WriteLine("Инвентарные объекты:");
                foreach (var item in inventoryItems)
                {
                    Console.WriteLine($"{item.Name} — Инв. №: {item.Number}");
                }
            }
        }

        private void ListAnimals()
        {
            var animals = _zoo.GetAnimals();
            if (animals.Count == 0)
            {
                Console.WriteLine("В зоопарке пока нет животных.");
            }
            else
            {
                Console.WriteLine("Животные в зоопарке:");
                foreach (var animal in animals)
                {
                    Console.WriteLine(animal.GetInfo());
                }
            }
        }

        private void ListThings()
        {
            var things = _zoo.GetThings();
            if (things.Count == 0)
            {
                Console.WriteLine("Нет добавленных вещей.");
            }
            else
            {
                Console.WriteLine("Вещи в зоопарке:");
                foreach (var thing in things)
                {
                    Console.WriteLine(thing.GetInfo());
                }
            }
        }

        private int ReadInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                    break;
                else
                    Console.WriteLine("Неверное число. Попробуйте снова.");
            }
            return value;
        }
    }
}
