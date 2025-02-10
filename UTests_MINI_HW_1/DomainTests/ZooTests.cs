using Xunit;
using Moq;
using MINI_HW_1.Domain;
using MINI_HW_1.Domain.Animals;
using MINI_HW_1.Domain.Things;
using MINI_HW_1.Interfaces;

namespace UTests_MINI_HW_1.DomainTests
{
    [Collection("Sequential Console Tests")]
    public class ZooTests
    {
        [Fact]
        public void AddAnimal_Should_AddAnimal_When_VeterinaryClinicApproves()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(true);
            var zoo = new Zoo(mockVet.Object);
            var rabbit = new Rabbit("TestRabbit", 2, 7);

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
            var rabbit = new Rabbit("TestRabbit", 2, 7);

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
            var rabbit = new Rabbit("Rabbit", 2, 7);
            var tiger = new Tiger("Tiger", 10);
            zoo.AddAnimal(rabbit);
            zoo.AddAnimal(tiger);

            // Act
            int totalFood = zoo.GetTotalFoodConsumption();

            // Assert
            Assert.Equal(12, totalFood);
        }

        [Fact]
        public void GetContactZooCandidates_Should_Return_Only_Herbo_With_KindnessGreaterThan5()
        {
            // Arrange
            var mockVet = new Mock<IVeterinaryClinic>();
            mockVet.Setup(v => v.CheckAnimal(It.IsAny<Animal>())).Returns(true);
            var zoo = new Zoo(mockVet.Object);
            var rabbit1 = new Rabbit("Rabbit1", 2, 4); // не подходит
            var rabbit2 = new Rabbit("Rabbit2", 3, 7); // подходит
            var tiger = new Tiger("Tiger", 10);         // не травоядное
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
            var rabbit = new Rabbit("Rabbit", 2, 7);
            var tiger = new Tiger("Tiger", 10);
            zoo.AddAnimal(rabbit);
            zoo.AddAnimal(tiger);

            // Создадим вещь
            var table = new Table("TestTable");
            table.Number = 999; // для теста устанавливаем произвольный инвентарный номер
            zoo.AddThing(table);

            // Act
            var items = zoo.GetAllInventoryItems();

            // Assert
            Assert.Equal(3, items.Count);
        }
    }
}
