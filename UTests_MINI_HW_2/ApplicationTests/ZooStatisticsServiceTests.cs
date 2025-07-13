using MINI_HW_2.Application.Services;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Enclosures;
using MINI_HW_2.Infrastructure.Repositories;

namespace UTests_MINI_HW_2.ApplicationTests
{
    public class ZooStatisticsServiceTests
    {
        private readonly ZooStatisticsService _service;
        private readonly InMemoryAnimalRepository _animalRepo;
        private readonly InMemoryEnclosureRepository _enclosureRepo;

        public ZooStatisticsServiceTests()
        {
            _animalRepo = new InMemoryAnimalRepository();
            _enclosureRepo = new InMemoryEnclosureRepository();
            _service = new ZooStatisticsService(_animalRepo, _enclosureRepo);
        }

        [Fact]
        public void GetTotalAnimals_Empty_ReturnsZero()
        {
            // Arrange: no animals
            // Act
            var total = _service.GetTotalAnimals();
            // Assert
            Assert.Equal(0, total);
        }

        [Fact]
        public void GetTotalAnimals_NonEmpty_ReturnsCount()
        {
            // Arrange
            _animalRepo.Add(new Rabbit("Belyash", DateTime.UtcNow, Sex.Unknown, "Морковь", 7));
            _animalRepo.Add(new Wolf("Wolfy", DateTime.UtcNow, Sex.Male, "Мясо"));

            // Act
            var total = _service.GetTotalAnimals();

            // Assert
            Assert.Equal(2, total);
        }

        [Fact]
        public void GetAnimalsBySpecies()
        {
            // Arrange
            _animalRepo.Add(new Tiger("Tiger1", DateTime.UtcNow, Sex.Male, "Мясо"));
            _animalRepo.Add(new Tiger("Tiger2", DateTime.UtcNow, Sex.Female, "Мясо"));
            _animalRepo.Add(new Rabbit("Rabbit1", DateTime.UtcNow, Sex.Unknown, "Морковь", 5));

            // Act
            var dict = _service.GetAnimalsBySpecies();

            // Assert
            Assert.Equal(2, dict["Tiger"]);
            Assert.Equal(1, dict["Rabbit"]);
        }

        [Fact]
        public void GetHealthStats_HealthyAndSick()
        {
            // Arrange
            var rabbit = new Rabbit("R1", DateTime.UtcNow, Sex.Female, "Морковь", 5);
            var tiger = new Tiger("T1", DateTime.UtcNow, Sex.Male, "Мясо");
            // Предположим, что у нас есть способ отмечать статус как Sick
            // (например, tiger.HealthStatus = HealthStatus.Sick)
            tiger.GetType().GetProperty("HealthStatus")?.SetValue(tiger, HealthStatus.Sick);

            _animalRepo.Add(rabbit);
            _animalRepo.Add(tiger);

            // Act
            var (healthy, sick) = _service.GetHealthStats();

            // Assert
            Assert.Equal(1, healthy);
            Assert.Equal(1, sick);
        }

        [Fact]
        public void GetEnclosuresOccupancy()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Herbivore, 100, 2);
            _enclosureRepo.Add(enclosure);

            // имитируем что AnimalIds.Count=1
            enclosure.AddAnimal(Guid.NewGuid());
            _enclosureRepo.Update(enclosure);

            // Act
            var occupancy = _service.GetEnclosuresOccupancy();

            // Assert
            Assert.Single(occupancy);
            var ratio = occupancy[enclosure.Id];
            Assert.Equal(0.5, ratio, 3); // 1/2 = 0.5
        }
    }
}
