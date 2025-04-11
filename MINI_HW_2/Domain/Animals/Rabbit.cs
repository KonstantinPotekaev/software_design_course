using System;

namespace MINI_HW_2.Domain.Animals
{
    public class Rabbit : Herbo
    {
        public Rabbit(string name, DateTime birthDate, Sex sex, string favoriteFood, int kindness)
            : base("Rabbit", name, birthDate, sex, favoriteFood, food: ParseFood(favoriteFood), kindness: kindness)
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