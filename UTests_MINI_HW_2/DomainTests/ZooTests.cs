using Moq;
using MINI_HW_2.Interfaces;
using MINI_HW_2.Domain;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Things;

namespace UTests_MINI_HW_2.DomainTests
{
    public class ZooTests
    {
        [Fact]
        public void AddAnimal_Should_AddAnimal_When_VeterinaryClinicApproves()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(true);
            var zoo = new Zoo(mockVet.Object);
            var rabbit = new Rabbit("TestRabbit", DateTime.UtcNow, Sex.Unknown, "2", 7);

            // Act
            bool result = zoo.AddAnimal(rabbit);

            // Assert
            Assert.True(result);
            List<Animal> animals = zoo.GetAnimals();
            Assert.Single(animals);
            Assert.Equal(1, rabbit.Number);
        }

        [Fact]
        public void AddAnimal_Should_NotAddAnimal_When_VeterinaryClinicDeclines()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(false);
            var zoo = new Zoo(mockVet.Object);
            var rabbit = new Rabbit("TestRabbit", DateTime.UtcNow, Sex.Unknown, "2", 7);

            // Act
            bool result = zoo.AddAnimal(rabbit);

            // Assert
            Assert.False(result);
            List<Animal> animals = zoo.GetAnimals();
            Assert.Empty(animals);
        }

        [Fact]
        public void GetTotalFoodConsumption_Should_ReturnSumOfFood_ForAllAnimals()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(true);
            var zoo = new Zoo(mockVet.Object);
            var rabbit = new Rabbit("Rabbit", DateTime.UtcNow, Sex.Unknown, "2", 7);
            var tiger = new Tiger("Tiger", DateTime.UtcNow, Sex.Male, "10");
            zoo.AddAnimal(rabbit);
            zoo.AddAnimal(tiger);

            // Act
            int totalFood = zoo.GetTotalFoodConsumption();

            // Assert: ожидается 2 + 10 = 12
            Assert.Equal(12, totalFood);
        }

        [Fact]
        public void GetContactZooCandidates_Should_Return_Only_Herbo_With_KindnessGreaterThan5()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(true);
            var zoo = new Zoo(mockVet.Object);
            var rabbit1 = new Rabbit("Rabbit1", DateTime.UtcNow, Sex.Unknown, "2", 4); // доброта = 4 ≤ 5
            var rabbit2 = new Rabbit("Rabbit2", DateTime.UtcNow, Sex.Unknown, "3", 7); // доброта = 7 > 5
            var tiger = new Tiger("Tiger", DateTime.UtcNow, Sex.Male, "10"); // не наследует Herbo
            zoo.AddAnimal(rabbit1);
            zoo.AddAnimal(rabbit2);
            zoo.AddAnimal(tiger);

            // Act
            List<Herbo> candidates = zoo.GetContactZooCandidates();

            // Assert
            Assert.Single(candidates);
            Assert.Equal("Rabbit2", candidates[0].Name);
        }

        [Fact]
        public void GetAllInventoryItems_Should_Return_Both_Animals_And_Things()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(true);
            var zoo = new Zoo(mockVet.Object);
            var rabbit = new Rabbit("Rabbit", DateTime.UtcNow, Sex.Unknown, "2", 7);
            var tiger = new Tiger("Tiger", DateTime.UtcNow, Sex.Male, "10");
            zoo.AddAnimal(rabbit);
            zoo.AddAnimal(tiger);

            var table = new Table("TestTable");
            zoo.AddThing(table);

            // Act
            var items = zoo.GetAllInventoryItems();

            // Assert: животных – 2, вещей – 1, итого 3 элемента
            Assert.Equal(3, items.Count);
        }
    }
}