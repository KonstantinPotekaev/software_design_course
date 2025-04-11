using System;

namespace MINI_HW_2.Domain.Animals
{
    public abstract class Predator : Animal
    {
        protected Predator(string species, string name, DateTime birthDate, Sex sex, string favoriteFood, int food)
            : base(species, name, birthDate, sex, favoriteFood, food)
        {
        }
    }
}