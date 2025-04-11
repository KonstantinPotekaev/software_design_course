using System;
using System.Collections.Generic;
using MINI_HW_2.Domain.Feeding;

namespace MINI_HW_2.Infrastructure.Repositories
{
    public interface IFeedingScheduleRepository
    {
        void Add(FeedingSchedule schedule);
        FeedingSchedule? GetById(Guid id);
        IEnumerable<FeedingSchedule> GetAll();
        IEnumerable<FeedingSchedule> GetAllDue(DateTime now);
        void Update(FeedingSchedule schedule);
        void Delete(Guid id);
    }
}