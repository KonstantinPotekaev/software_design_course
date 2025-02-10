using MINI_HW_1.Domain.Animals;
using MINI_HW_1.Utils;

namespace MINI_HW_1.AnimalCreators
{
    public class TigerCreator : IAnimalCreator
    {
        public string AnimalTypeName => "Тигр (хищник)";

        public Animal? CreateAnimal()
        {
            var name = InputHelper.ReadNonEmptyString("Введите имя тигра (или 'q' для отмены): ");
            if (name == null)
            {
                System.Console.WriteLine("Операция создания тигра отменена.");
                return null;
            }

            var food = InputHelper.ReadInt("Введите количество кг еды в день (или 'q' для отмены): ");
            if (food == null)
            {
                System.Console.WriteLine("Операция создания тигра отменена.");
                return null;
            }

            return new Tiger(name, food.Value);
        }
    }
}