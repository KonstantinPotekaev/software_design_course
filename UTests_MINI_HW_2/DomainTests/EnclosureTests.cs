using MINI_HW_2.Domain.Enclosures;

namespace UTests_MINI_HW_2.DomainTests
{
    public class EnclosureTests
    {
        [Fact]
        public void AddAnimal_WithinCapacity_Success()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Herbivore, 100, 2);
            var animalId = Guid.NewGuid();

            // Act
            bool added = enclosure.AddAnimal(animalId);

            // Assert
            Assert.True(added);
            Assert.Contains(animalId, enclosure.AnimalIds);
        }

        [Fact]
        public void AddAnimal_ExceedCapacity_Fails()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Carnivore, 50, 0);
            var animalId = Guid.NewGuid();

            // Act
            bool added = enclosure.AddAnimal(animalId);

            // Assert
            Assert.False(added);
            Assert.DoesNotContain(animalId, enclosure.AnimalIds);
        }

        [Fact]
        public void RemoveAnimal_Existing_Removes()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Herbivore, 100, 3);
            var animalId = Guid.NewGuid();
            enclosure.AddAnimal(animalId);

            // Act
            bool removed = enclosure.RemoveAnimal(animalId);

            // Assert
            Assert.True(removed);
            Assert.DoesNotContain(animalId, enclosure.AnimalIds);
        }

        [Fact]
        public void Clean_SetsLastCleaned()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Mixed, 120, 5);

            // Act
            enclosure.Clean();

            // Assert
            Assert.NotNull(enclosure.LastCleaned);
            Assert.True((DateTime.UtcNow - enclosure.LastCleaned.Value).TotalSeconds < 5);
        }
    }
}