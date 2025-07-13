using MINI_HW_2.Interfaces;

namespace MINI_HW_2.Domain.Things
{
    public abstract class Thing : IInventory
    {
        public string Name { get; protected set; }
        public int Number { get; set; }

        protected Thing(string name)
        {
            Name = name;
        }

        public virtual string GetInfo() =>
            $"{Name} (Инв. №: {Number})";
    }
}