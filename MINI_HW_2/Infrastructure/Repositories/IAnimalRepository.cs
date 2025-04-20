using System;
using System.Collections.Generic;
using MINI_HW_2.Domain.Animals;

namespace MINI_HW_2.Infrastructure.Repositories
{
    public interface IAnimalRepository
    {
        void Add(Animal animal);
        Animal GetById(Guid id);
        IEnumerable<Animal> GetAll();
        void Update(Animal animal);
        void Delete(Guid id);
    }
}