using System;
using System.Collections.Generic;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Events;
using MINI_HW_2.Domain.Feeding;
using MINI_HW_2.Infrastructure.Repositories;

namespace MINI_HW_2.Application.Services
{
    public class FeedingOrganizationService
    {
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly IAnimalRepository _animalRepository;

        public FeedingOrganizationService(IFeedingScheduleRepository feedingScheduleRepository,
            IAnimalRepository animalRepository)
        {
            _feedingScheduleRepository = feedingScheduleRepository;
            _animalRepository = animalRepository;
        }

        public FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingTime, string foodType)
        {
            var animal = _animalRepository.GetById(animalId);
            if (animal == null)
                throw new Exception("Животное не найдено.");
            var schedule = new FeedingSchedule(animalId, feedingTime, foodType);
            _feedingScheduleRepository.Add(schedule);
            return schedule;
        }

        public List<FeedingSchedule> ProcessDueFeedings()
        {
            var dueSchedules = new List<FeedingSchedule>(_feedingScheduleRepository.GetAllDue(DateTime.UtcNow));
            foreach (var schedule in dueSchedules)
            {
                var animal = _animalRepository.GetById(schedule.AnimalId);
                if (animal != null && !schedule.IsCompleted)
                {
                    animal.Feed();
                    schedule.MarkCompleted();
                    _feedingScheduleRepository.Update(schedule);

                    var evt = new FeedingTimeEvent(schedule.Id, schedule.AnimalId, schedule.FeedingTime);
                    System.Console.WriteLine(
                        $"FeedingTimeEvent: Животное {schedule.AnimalId} покормлено в {schedule.FeedingTime}.");
                }
            }

            return dueSchedules;
        }
    }
}