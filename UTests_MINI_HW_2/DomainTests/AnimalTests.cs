using MINI_HW_2.Domain.Animals;

namespace UTests_MINI_HW_2.DomainTests
{
    public class AnimalTests
    {
        [Fact]
        public void Feed_NoException()
        {
            // Arrange
            var tiger = new Tiger("Тигрин", DateTime.UtcNow, Sex.Male, "Мясо");

            // Act (просто проверяем что не упадёт)
            tiger.Feed();

            // Assert
            // Нет особого assert, т.к. метод Feed() не возвращает ничего; 
            // можно проверить внутреннее состояние, если оно хранится
        }

        [Fact]
        public void Treat_SickBecomesHealthy()
        {
            // Arrange
            var wolf = new Wolf("Волчок", DateTime.UtcNow, Sex.Male, "Мясо");
            // предположим, что можно отразить больное состояние
            wolf.GetType().GetProperty("HealthStatus")?.SetValue(wolf, HealthStatus.Sick);

            // Act
            wolf.Treat();

            // Assert
            Assert.Equal(HealthStatus.Healthy, wolf.HealthStatus);
        }

        [Fact]
        public void MoveToEnclosure_AssignsId()
        {
            // Arrange
            var monkey = new Monkey("Кеша", DateTime.UtcNow, Sex.Female, "Фрукты");

            var enclosureId = Guid.NewGuid();

            // Act
            monkey.MoveToEnclosure(enclosureId);

            // Assert
            Assert.Equal(enclosureId, monkey.EnclosureId);
        }
    }
}