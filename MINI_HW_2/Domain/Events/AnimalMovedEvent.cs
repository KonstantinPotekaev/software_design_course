using System;

namespace MINI_HW_2.Domain.Events
{
    public class AnimalMovedEvent
    {
        public Guid AnimalId { get; }
        public Guid FromEnclosureId { get; }
        public Guid ToEnclosureId { get; }
        public DateTime Timestamp { get; }

        public AnimalMovedEvent(Guid animalId, Guid fromEnclosureId, Guid toEnclosureId)
        {
            AnimalId = animalId;
            FromEnclosureId = fromEnclosureId;
            ToEnclosureId = toEnclosureId;
            Timestamp = DateTime.UtcNow;
        }
    }
}