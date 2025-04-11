using System;
using System.Collections.Generic;
using System.Linq;
using MINI_HW_2.Domain.Feeding;

namespace MINI_HW_2.Infrastructure.Repositories
{
    public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly List<FeedingSchedule> _schedules = new List<FeedingSchedule>();

        public void Add(FeedingSchedule schedule) => _schedules.Add(schedule);

        public FeedingSchedule GetById(Guid id) => _schedules.FirstOrDefault(s => s.Id == id);

        public IEnumerable<FeedingSchedule> GetAll() => _schedules;

        public IEnumerable<FeedingSchedule> GetAllDue(DateTime now) =>
            _schedules.Where(s => s.FeedingTime <= now && !s.IsCompleted);

        public void Update(FeedingSchedule schedule)
        {
            var index = _schedules.FindIndex(s => s.Id == schedule.Id);
            if (index >= 0)
                _schedules[index] = schedule;
        }

        public void Delete(Guid id)
        {
            var schedule = GetById(id);
            if (schedule != null)
                _schedules.Remove(schedule);
        }
    }
}