using System;
using MINI_HW_1.Domain.Things;
using MINI_HW_1.ThingCreators;

namespace MINI_HW_1.ThingCreators
{
    public class ComputerCreator : IThingCreator
    {
        public string ThingTypeName => "Компьютер";

        public Thing CreateThing()
        {
            Console.Write("Введите наименование компьютера: ");
            string name = Console.ReadLine();
            return new Computer(name);
        }
    }
}