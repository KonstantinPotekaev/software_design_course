using System;

namespace MINI_HW_2.Domain.Feeding
{
    public class FeedingSchedule
    {
        public Guid Id { get; private set; }
        public Guid AnimalId { get; private set; }
        public DateTime FeedingTime { get; private set; }
        public string FoodType { get; private set; }
        public bool IsCompleted { get; private set; }

        public FeedingSchedule(Guid animalId, DateTime feedingTime, string foodType)
        {
            Id = Guid.NewGuid();
            AnimalId = animalId;
            FeedingTime = feedingTime;
            FoodType = foodType;
            IsCompleted = false;
        }

        public void Reschedule(DateTime newTime)
        {
            if (newTime < DateTime.UtcNow)
                throw new ArgumentException("Новое время кормления не может быть в прошлом.", nameof(newTime));
            FeedingTime = newTime;
        }

        public void MarkCompleted()
        {
            IsCompleted = true;
        }

        public object GetInfo() => new
        {
            Id,
            AnimalId,
            FeedingTime,
            FoodType,
            IsCompleted
        };
    }
}