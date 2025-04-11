using System;
using System.Collections.Generic;
using System.Linq;
using MINI_HW_2.Domain.Enclosures;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Infrastructure.Repositories;

namespace MINI_HW_2.Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;

        public ZooStatisticsService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }

        public int GetTotalAnimals() =>
            _animalRepository.GetAll().Count();

        public Dictionary<string, int> GetAnimalsBySpecies() =>
            _animalRepository.GetAll().GroupBy(a => a.Species)
                .ToDictionary(g => g.Key, g => g.Count());

        public (int healthy, int sick) GetHealthStats()
        {
            var animals = _animalRepository.GetAll();
            int healthy = animals.Count(a => a.HealthStatus == HealthStatus.Healthy);
            int sick = animals.Count(a => a.HealthStatus == HealthStatus.Sick);
            return (healthy, sick);
        }

        public int GetTotalEnclosures() =>
            _enclosureRepository.GetAll().Count();

        public Dictionary<Guid, double> GetEnclosuresOccupancy()
        {
            var dict = new Dictionary<Guid, double>();
            foreach (var enclosure in _enclosureRepository.GetAll())
            {
                double occupancy = (double)enclosure.AnimalIds.Count / enclosure.Capacity;
                dict[enclosure.Id] = occupancy;
            }

            return dict;
        }
    }
}