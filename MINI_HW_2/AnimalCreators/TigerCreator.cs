using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Utils;

namespace MINI_HW_2.AnimalCreators
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

            var food = InputHelper.ReadInt("Введите количество кг еды в день (или 'q' для отмены): ", rangeStart: 0);
            if (food == null)
            {
                System.Console.WriteLine("Операция создания тигра отменена.");
                return null;
            }

            return new Tiger(name, System.DateTime.UtcNow, Sex.Unknown, $"Еда: {food.Value} кг");
        }
    }
}