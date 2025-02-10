using System;
using MINI_HW_1.Interfaces;
using MINI_HW_1.Domain.Animals;

namespace MINI_HW_1.Services
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