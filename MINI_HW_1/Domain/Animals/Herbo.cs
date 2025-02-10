namespace MINI_HW_1.Domain.Animals
{
    public abstract class Herbo : Animal
    {
        public int Kindness { get; protected set; }

        protected Herbo(string name, int food, int kindness)
            : base(name, food)
        {
            Kindness = kindness;
        }

        public override string GetInfo() =>
            $"{base.GetInfo()}, доброта: {Kindness}";
    }
}