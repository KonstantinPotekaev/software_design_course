using System;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Utils;

namespace MINI_HW_2.AnimalCreators
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
                    System.Console.WriteLine("Операция создания кролика отменена.");
                    return null;
                }

                var food = InputHelper.ReadInt("Введите количество кг еды в день (или 'q' для отмены): ", rangeStart: 0);
                if (food == null)
                {
                    System.Console.WriteLine("Операция создания кролика отменена.");
                    return null;
                }

                var kindness = InputHelper.ReadInt("Введите уровень доброты (от 1 до 10) (или 'q' для отмены): ", rangeStart: 1, rangeEnd: 10);
                if (kindness == null)
                {
                    System.Console.WriteLine("Операция создания кролика отменена.");
                    return null;
                }

                try
                {
                    return new Rabbit(name, DateTime.UtcNow, Sex.Unknown, $"Еда: {food.Value} кг", kindness.Value);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    System.Console.WriteLine("Ошибка: " + ex.Message);
                    System.Console.WriteLine("Пожалуйста, введите корректное значение для уровня доброты.");
                }
            }
        }
    }
}