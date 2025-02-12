using MINI_HW_1.Domain.Animals;
using MINI_HW_1.Utils;

namespace MINI_HW_1.AnimalCreators
{
    public class WolfCreator : IAnimalCreator
    {
        public string AnimalTypeName => "Волк (хищник)";

        public Animal? CreateAnimal()
        {
            var name = InputHelper.ReadNonEmptyString("Введите имя волка (или 'q' для отмены): ");
            if (name == null)
            {
                Console.WriteLine("Операция создания волка отменена.");
                return null;
            }

            var food = InputHelper.ReadInt(prompt: "Введите количество кг еды в день (или 'q' для отмены): ", rangeStart: 0);
            if (food == null)
            {
                Console.WriteLine("Операция создания волка отменена.");
                return null;
            }

            return new Wolf(name, food.Value);
        }
    }
}