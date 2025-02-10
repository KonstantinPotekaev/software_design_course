using System;
using MINI_HW_1.Domain.Animals;
using MINI_HW_1.Utils;

namespace MINI_HW_1.AnimalCreators
{
    public class RabbitCreator : IAnimalCreator
    {
        public string AnimalTypeName => "Кролик (травоядный)";

        public Animal? CreateAnimal()
        {
            while (true)
            {
                var name = InputHelper.ReadNonEmptyString("Введите имя кролика (или 'q' для отмены): ");
                if (name == null)
                {
                    Console.WriteLine("Операция создания кролика отменена.");
                    return null;
                }

                var food = InputHelper.ReadInt("Введите количество кг еды в день (или 'q' для отмены): ");
                if (food == null)
                {
                    Console.WriteLine("Операция создания кролика отменена.");
                    return null;
                }

                var kindness = InputHelper.ReadInt("Введите уровень доброты (от 1 до 10) (или 'q' для отмены): ");
                if (kindness == null)
                {
                    Console.WriteLine("Операция создания кролика отменена.");
                    return null;
                }

                try
                {
                    return new Rabbit(name, food.Value, kindness.Value);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    Console.WriteLine("Пожалуйста, введите корректное значение для уровня доброты.");
                }
            }
        }
    }
}