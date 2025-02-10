using System;
using MINI_HW_1.AnimalCreators;
using MINI_HW_1.Domain.Animals;

namespace MINI_HW_1.AnimalCreators
{
    public class MonkeyCreator : IAnimalCreator
    {
        public string AnimalTypeName => "Обезьяна";

        public Animal CreateAnimal()
        {
            Console.Write("Введите имя обезьяны: ");
            string name = Console.ReadLine();
            int food = ReadInt("Введите количество кг еды в день: ");
            return new Monkey(name, food);
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