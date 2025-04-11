using MINI_HW_2.Domain.Animals;

namespace MINI_HW_2.AnimalCreators
{
    public interface IAnimalCreator
    {
        string AnimalTypeName { get; }
        Animal? CreateAnimal();
    }
}