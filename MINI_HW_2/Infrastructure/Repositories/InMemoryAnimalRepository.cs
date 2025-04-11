using System;
using System.Collections.Generic;
using System.Linq;
using MINI_HW_2.Domain.Animals;

namespace MINI_HW_2.Infrastructure.Repositories
{
    public class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new List<Animal>();

        public void Add(Animal animal) => _animals.Add(animal);

        public Animal GetById(Guid id) => _animals.FirstOrDefault(a => a.Id == id);

        public IEnumerable<Animal> GetAll() => _animals;

        public void Update(Animal animal)
        {
            var index = _animals.FindIndex(a => a.Id == animal.Id);
            if (index >= 0)
                _animals[index] = animal;
        }

        public void Delete(Guid id)
        {
            var animal = GetById(id);
            if (animal != null)
                _animals.Remove(animal);
        }
    }
}