using System;
using MINI_HW_1.AnimalCreators;
using MINI_HW_1.Domain.Animals;

namespace MINI_HW_1.AnimalCreators
{
    public class RabbitCreator : IAnimalCreator
    {
        public string AnimalTypeName => "Кролик (травоядный)";

        public Animal CreateAnimal()
        {
            Console.Write("Введите имя кролика: ");
            string name = Console.ReadLine();
            int food = ReadInt("Введите количество кг еды в день: ");
            int kindness = ReadInt("Введите уровень доброты (от 1 до 10): ");
            return new Rabbit(name, food, kindness);
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