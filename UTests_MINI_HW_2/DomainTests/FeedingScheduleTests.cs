using MINI_HW_2.Domain.Feeding;

namespace UTests_MINI_HW_2.DomainTests
{
    public class FeedingScheduleTests
    {
        [Fact]
        public void MarkCompleted_SetsIsCompleted()
        {
            // Arrange
            var schedule = new FeedingSchedule(Guid.NewGuid(), DateTime.UtcNow.AddHours(1), "Мясо");

            // Act
            schedule.MarkCompleted();

            // Assert
            Assert.True(schedule.IsCompleted);
        }

        [Fact]
        public void Reschedule_Valid_ChangesTime()
        {
            // Arrange
            var schedule = new FeedingSchedule(Guid.NewGuid(), DateTime.UtcNow.AddHours(1), "Мясо");
            var newTime = DateTime.UtcNow.AddHours(3);

            // Act
            schedule.Reschedule(newTime);

            // Assert
            Assert.Equal(newTime, schedule.FeedingTime);
        }

        [Fact]
        public void Reschedule_Past_Throws()
        {
            // Arrange
            var schedule = new FeedingSchedule(Guid.NewGuid(), DateTime.UtcNow.AddHours(2), "Мясо");
            var pastTime = DateTime.UtcNow.AddMinutes(-10);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => schedule.Reschedule(pastTime));
            Assert.Contains("не может быть в прошлом", ex.Message.ToLower());
        }
    }
}