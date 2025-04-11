using MINI_HW_2.Domain.Things;
using MINI_HW_2.Utils;

namespace MINI_HW_2.ThingCreators
{
    public class ComputerCreator : IThingCreator
    {
        public string ThingTypeName => "Компьютер";

        public Thing? CreateThing()
        {
            var name = InputHelper.ReadNonEmptyString("Введите наименование компьютера (или 'q' для отмены): ");
            if (name != null) return new Computer(name);
            Console.WriteLine("Операция создания компьютера отменена.");
            return null;
        }
    }
}