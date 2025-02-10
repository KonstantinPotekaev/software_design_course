using MINI_HW_1.Interfaces;

namespace MINI_HW_1.Domain.Animals
{
    public abstract class Animal : IAlive, IInventory
    {
        public string Name { get; protected set; }
        public int Food { get; protected set; }
        public int Number { get; set; }

        protected Animal(string name, int food)
        {
            Name = name;
            Food = food;
        }

        public virtual string GetInfo() =>
            $"{Name} (Инв. №: {Number}), еда: {Food} кг/день";
    }
}