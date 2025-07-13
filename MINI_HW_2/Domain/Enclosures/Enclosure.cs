using System;
using System.Collections.Generic;

namespace MINI_HW_2.Domain.Enclosures
{
    public enum EnclosureType
    {
        Carnivore,
        Herbivore,
        Mixed,
        Bird,
        Aquarium
    }

    public class Enclosure
    {
        public Guid Id { get; private set; }
        public EnclosureType Type { get; private set; }
        public double Size { get; private set; }
        public int Capacity { get; private set; }
        public List<Guid> AnimalIds { get; private set; }
        public DateTime? LastCleaned { get; private set; }

        public Enclosure(EnclosureType type, double size, int capacity)
        {
            Id = Guid.NewGuid();
            Type = type;
            Size = size;
            Capacity = capacity;
            AnimalIds = new List<Guid>();
        }

        public bool AddAnimal(Guid animalId)
        {
            if (AnimalIds.Count >= Capacity)
                return false;
            AnimalIds.Add(animalId);
            return true;
        }

        public bool RemoveAnimal(Guid animalId)
        {
            return AnimalIds.Remove(animalId);
        }

        public void Clean()
        {
            LastCleaned = DateTime.UtcNow;
        }

        public object GetInfo() => new
        {
            Id,
            Type,
            Size,
            Capacity,
            CurrentCount = AnimalIds.Count,
            LastCleaned
        };
    }
}