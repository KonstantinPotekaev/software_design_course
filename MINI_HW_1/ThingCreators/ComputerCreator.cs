using System;
using MINI_HW_1.Domain.Things;
using MINI_HW_1.Utils;

namespace MINI_HW_1.ThingCreators
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