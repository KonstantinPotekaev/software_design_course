using MINI_HW_2.Application.Services;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Enclosures;
using MINI_HW_2.Infrastructure.Repositories;

namespace UTests_MINI_HW_2.ApplicationTests
{
    public class AnimalTransferServiceTests
    {
        private readonly AnimalTransferService _service;
        private readonly InMemoryAnimalRepository _animalRepo;
        private readonly InMemoryEnclosureRepository _enclosureRepo;

        public AnimalTransferServiceTests()
        {
            // Создаем in-memory репозитории и сервис
            _animalRepo = new InMemoryAnimalRepository();
            _enclosureRepo = new InMemoryEnclosureRepository();
            _service = new AnimalTransferService(_animalRepo, _enclosureRepo);
        }

        [Fact]
        public void TransferAnimal_Valid_Success()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Herbivore, 100.0, 2);
            _enclosureRepo.Add(enclosure);

            var rabbit = new Rabbit("Bunny", DateTime.UtcNow, Sex.Unknown, "Морковь", 7);
            _animalRepo.Add(rabbit);

            // Act
            _service.TransferAnimal(rabbit.Id, enclosure.Id);

            // Assert
            Assert.Equal(enclosure.Id, rabbit.EnclosureId);
            Assert.Single(enclosure.AnimalIds);
            Assert.Contains(rabbit.Id, enclosure.AnimalIds);
        }

        [Fact]
        public void TransferAnimal_UnknownAnimal_Throws()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Herbivore, 50, 2);
            _enclosureRepo.Add(enclosure);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => 
                _service.TransferAnimal(Guid.NewGuid(), enclosure.Id));
            Assert.Contains("не найдено", ex.Message.ToLower());
        }

        [Fact]
        public void TransferAnimal_UnknownEnclosure_Throws()
        {
            // Arrange
            var rabbit = new Rabbit("Bunny", DateTime.UtcNow, Sex.Unknown, "Морковь", 5);
            _animalRepo.Add(rabbit);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                _service.TransferAnimal(rabbit.Id, Guid.NewGuid()));
            Assert.Contains("вольер не найден", ex.Message.ToLower());
        }

        [Fact]
        public void TransferAnimal_EnclosureFull_Throws()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Carnivore, 50, 0); // capacity=0
            _enclosureRepo.Add(enclosure);

            var tiger = new Tiger("Тигрин", DateTime.UtcNow, Sex.Male, "Мясо");
            _animalRepo.Add(tiger);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                _service.TransferAnimal(tiger.Id, enclosure.Id));
            Assert.Contains("заполнен", ex.Message.ToLower());
        }
    }
}
