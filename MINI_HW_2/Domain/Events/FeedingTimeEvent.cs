using System;
using MINI_HW_2.Domain.Feeding;

namespace MINI_HW_2.Domain.Events
{
    public class FeedingTimeEvent
    {
        public Guid FeedingScheduleId { get; }
        public Guid AnimalId { get; }
        public DateTime FeedingTime { get; }

        public FeedingTimeEvent(Guid feedingScheduleId, Guid animalId, DateTime feedingTime)
        {
            FeedingScheduleId = feedingScheduleId;
            AnimalId = animalId;
            FeedingTime = feedingTime;
        }
    }
}