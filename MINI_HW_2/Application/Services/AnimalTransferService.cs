using System;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Enclosures;
using MINI_HW_2.Domain.Events;
using MINI_HW_2.Infrastructure.Repositories;

namespace MINI_HW_2.Application.Services
{
    public class AnimalTransferService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;

        public AnimalTransferService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }

        public void TransferAnimal(Guid animalId, Guid targetEnclosureId)
        {
            var animal = _animalRepository.GetById(animalId);
            if (animal == null)
                throw new Exception("Животное не найдено.");
            var currentEnclosureId = animal.EnclosureId;
            var targetEnclosure = _enclosureRepository.GetById(targetEnclosureId);
            if (targetEnclosure == null)
                throw new Exception("Целевой вольер не найден.");

            if (currentEnclosureId == targetEnclosureId)
                throw new Exception("Животное уже находится в целевом вольере.");

            if (targetEnclosure.AnimalIds.Count >= targetEnclosure.Capacity)
                throw new Exception("Целевой вольер заполнен.");

            if (currentEnclosureId.HasValue)
            {
                var currentEnclosure = _enclosureRepository.GetById(currentEnclosureId.Value);
                currentEnclosure?.RemoveAnimal(animalId);
                _enclosureRepository.Update(currentEnclosure);
            }

            bool added = targetEnclosure.AddAnimal(animalId);
            if (!added)
                throw new Exception("Не удалось добавить животное в целевой вольер.");

            animal.MoveToEnclosure(targetEnclosureId);
            _animalRepository.Update(animal);

            var evt = new AnimalMovedEvent(animalId, currentEnclosureId ?? Guid.Empty, targetEnclosureId);
            System.Console.WriteLine(
                $"AnimalMovedEvent: Животное {animalId} перемещено с {currentEnclosureId} в {targetEnclosureId} в {evt.Timestamp}.");
        }
    }
}