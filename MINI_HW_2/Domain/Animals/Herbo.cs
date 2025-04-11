using System;

namespace MINI_HW_2.Domain.Animals
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

        protected Herbo(string species, string name, DateTime birthDate, Sex sex, string favoriteFood, int food,
            int kindness)
            : base(species, name, birthDate, sex, favoriteFood, food)
        {
            Kindness = kindness;
        }

        public override object GetInfo() => new
        {
            Id,
            Species,
            Name,
            BirthDate,
            Sex,
            FavoriteFood,
            Food,
            HealthStatus,
            EnclosureId,
            Kindness,
            Number
        };
    }
}