using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Utils;

namespace MINI_HW_2.AnimalCreators
{
    public class WolfCreator : IAnimalCreator
    {
        public string AnimalTypeName => "Волк (хищник)";

        public Animal? CreateAnimal()
        {
            var name = InputHelper.ReadNonEmptyString("Введите имя волка (или 'q' для отмены): ");
            if (name == null)
            {
                System.Console.WriteLine("Операция создания волка отменена.");
                return null;
            }

            var food = InputHelper.ReadInt("Введите количество кг еды в день (или 'q' для отмены): ", rangeStart: 0);
            if (food == null)
            {
                System.Console.WriteLine("Операция создания волка отменена.");
                return null;
            }

            return new Wolf(name, System.DateTime.UtcNow, Sex.Unknown, $"Еда: {food.Value} кг");
        }
    }
}