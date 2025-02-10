namespace MINI_HW_1.Domain.Animals
{
    public abstract class Herbo : Animal
    {
        private int _kindness;

        public int Kindness 
        { 
            get => _kindness;
            protected set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentOutOfRangeException(nameof(Kindness), "Уровень доброты должен быть от 1 до 10.");
                _kindness = value;
            }
        }

        protected Herbo(string name, int food, int kindness)
            : base(name, food)
        {
            Kindness = kindness;
        }
    }
}