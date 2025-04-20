using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Interfaces;

namespace MINI_HW_2.Services
{
    public class VeterinaryClinic : IVeterinaryClinic
    {
        public bool CheckAnimal(Animal animal)
        {
            Console.WriteLine($"Проводится проверка животного: {animal.Name}");
            Console.Write("Животное здорово? (y/n): ");
            var input = Console.ReadLine();
            return input?.Trim().ToLower() == "y";
        }
    }
}