using System;
using MINI_HW_1.Domain.Things;
using MINI_HW_1.Utils;

namespace MINI_HW_1.ThingCreators
{
    public class TableCreator : IThingCreator
    {
        public string ThingTypeName => "Стол";

        public Thing? CreateThing()
        {
            var name = InputHelper.ReadNonEmptyString("Введите наименование стола (или 'q' для отмены): ");
            if (name != null) return new Table(name);
            Console.WriteLine("Операция отменена.");
            return null;
        }
    }
}