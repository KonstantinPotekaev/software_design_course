using MINI_HW_2.Application.Services;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Feeding;
using MINI_HW_2.Infrastructure.Repositories;

namespace UTests_MINI_HW_2.ApplicationTests
{
    public class FeedingOrganizationServiceTests
    {
        private readonly FeedingOrganizationService _service;
        private readonly InMemoryFeedingScheduleRepository _scheduleRepo;
        private readonly InMemoryAnimalRepository _animalRepo;

        public FeedingOrganizationServiceTests()
        {
            _scheduleRepo = new InMemoryFeedingScheduleRepository();
            _animalRepo = new InMemoryAnimalRepository();
            _service = new FeedingOrganizationService(_scheduleRepo, _animalRepo);
        }

        [Fact]
        public void ScheduleFeeding_Valid_Success()
        {
            // Arrange
            var tiger = new Tiger("Тигрин", DateTime.UtcNow, Sex.Male, "Мясо");
            _animalRepo.Add(tiger);
            var feedTime = DateTime.UtcNow.AddHours(2);

            // Act
            var schedule = _service.ScheduleFeeding(tiger.Id, feedTime, "Мясо для тигра");

            // Assert
            Assert.NotNull(schedule);
            Assert.Equal(tiger.Id, schedule.AnimalId);
            Assert.Equal(feedTime, schedule.FeedingTime);
            Assert.False(schedule.IsCompleted);
        }

        [Fact]
        public void ScheduleFeeding_UnknownAnimal_Throws()
        {
            // Arrange
            var feedTime = DateTime.UtcNow.AddHours(1);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
                _service.ScheduleFeeding(Guid.NewGuid(), feedTime, "Unknown animal food"));
            Assert.Contains("не найдено", ex.Message.ToLower());
        }

        [Fact]
        public void ProcessDueFeedings_ShouldFeedAllDue()
        {
            // Arrange
            var wolf = new Wolf("Волчок", DateTime.UtcNow, Sex.Male, "Мясо");
            _animalRepo.Add(wolf);

            var schedule1 = new FeedingSchedule(wolf.Id, DateTime.UtcNow.AddMinutes(-30), "Мясо"); // истёкшее
            var schedule2 = new FeedingSchedule(wolf.Id, DateTime.UtcNow.AddMinutes(30), "Мясо");  // ещё не настало
            _scheduleRepo.Add(schedule1);
            _scheduleRepo.Add(schedule2);

            // Act
            var processed = _service.ProcessDueFeedings();

            // Assert
            Assert.Single(processed); // только schedule1 должен быть обработан
            Assert.True(schedule1.IsCompleted);
            Assert.False(schedule2.IsCompleted);
        }

        [Fact]
        public void ProcessDueFeedings_AlreadyCompleted_NotProcessedAgain()
        {
            // Arrange
            var monkey = new Monkey("Кеша", DateTime.UtcNow, Sex.Unknown, "Фрукты");
            _animalRepo.Add(monkey);

            var schedule = new FeedingSchedule(monkey.Id, DateTime.UtcNow.AddMinutes(-1), "Фрукты");
            schedule.MarkCompleted(); // уже выполнен
            _scheduleRepo.Add(schedule);

            // Act
            var processed = _service.ProcessDueFeedings();

            // Assert
            Assert.Empty(processed);  // уже выполнено, повторно не обрабатываем
            Assert.True(schedule.IsCompleted);
        }
    }
}
