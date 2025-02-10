using System;
using MINI_HW_1.Domain.Things;
using MINI_HW_1.ThingCreators;

namespace MINI_HW_1.ThingCreators
{
    public class TableCreator : IThingCreator
    {
        public string ThingTypeName => "Стол";

        public Thing CreateThing()
        {
            Console.Write("Введите наименование стола: ");
            string name = Console.ReadLine();
            return new Table(name);
        }
    }
}