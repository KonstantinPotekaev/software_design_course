namespace MINI_HW_1.Domain.Animals
{
    public abstract class Predator : Animal
    {
        protected Predator(string name, int food)
            : base(name, food)
        {
        }
    }
}