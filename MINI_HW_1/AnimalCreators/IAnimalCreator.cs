using MINI_HW_1.Domain.Animals;

namespace MINI_HW_1.AnimalCreators
{
    public interface IAnimalCreator
    {
        string AnimalTypeName { get; }
        Animal CreateAnimal();
    }
}