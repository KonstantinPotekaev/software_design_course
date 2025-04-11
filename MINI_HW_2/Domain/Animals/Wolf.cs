using System;

namespace MINI_HW_2.Domain.Animals
{
    public class Wolf : Predator
    {
        public Wolf(string name, DateTime birthDate, Sex sex, string favoriteFood)
            : base("Wolf", name, birthDate, sex, favoriteFood, food: ParseFood(favoriteFood))
        {
        }

        private static int ParseFood(string foodDescription)
        {
            int res;
            if (int.TryParse(foodDescription.Replace("Еда:", "").Replace("кг", "").Trim(), out res))
                return res;
            return 0;
        }
    }
}