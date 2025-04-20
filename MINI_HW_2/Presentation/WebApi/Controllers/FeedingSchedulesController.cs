using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MINI_HW_2.Application.Services;
using MINI_HW_2.Domain.Feeding;
using MINI_HW_2.Infrastructure.Repositories;

namespace MINI_HW_2.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingSchedulesController : ControllerBase
    {
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly FeedingOrganizationService _feedingService;

        public FeedingSchedulesController(IFeedingScheduleRepository feedingScheduleRepository,
            FeedingOrganizationService feedingService)
        {
            _feedingScheduleRepository = feedingScheduleRepository;
            _feedingService = feedingService;
        }

        // GET: api/feedingschedules
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetFeedingSchedules()
        {
            var schedules = _feedingScheduleRepository.GetAll();
            var result = schedules.Select(schedule => schedule.GetInfo()).ToList();

            return Ok(result);
        }

        // GET: api/feedingschedules/{id}
        [HttpGet("{id}")]
        public ActionResult<object> GetFeedingSchedule(Guid id)
        {
            var schedule = _feedingScheduleRepository.GetById(id);
            if (schedule == null)
                return NotFound();
            return Ok(schedule.GetInfo());
        }

        // POST: api/feedingschedules
        [HttpPost]
        public ActionResult<object> CreateFeedingSchedule([FromBody] CreateFeedingScheduleDto dto)
        {
            try
            {
                var schedule = _feedingService.ScheduleFeeding(dto.AnimalId, dto.FeedingTime, dto.FoodType);
                return CreatedAtAction(nameof(GetFeedingSchedule), new { id = schedule.Id }, schedule.GetInfo());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/feedingschedules/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateFeedingSchedule(Guid id, [FromBody] UpdateFeedingScheduleDto dto)
        {
            var schedule = _feedingScheduleRepository.GetById(id);
            if (schedule == null)
                return NotFound();
            try
            {
                schedule.Reschedule(dto.NewFeedingTime);
                // Обновляем тип пищи (предполагаем, что свойство FoodType изменяемо)
                typeof(FeedingSchedule).GetProperty("FoodType").SetValue(schedule, dto.NewFoodType);
                _feedingScheduleRepository.Update(schedule);
                return Ok(schedule.GetInfo());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/feedingschedules/{id}/complete
        [HttpPost("{id}/complete")]
        public IActionResult CompleteFeedingSchedule(Guid id)
        {
            var schedule = _feedingScheduleRepository.GetById(id);
            if (schedule == null)
                return NotFound();
            schedule.MarkCompleted();
            _feedingScheduleRepository.Update(schedule);
            return Ok(schedule.GetInfo());
        }

        // POST: api/feedingschedules/process-due
        [HttpPost("process-due")]
        public ActionResult<IEnumerable<object>> ProcessDueFeedings()
        {
            var processed = _feedingService.ProcessDueFeedings();
            var result = new List<object>();
            foreach (var schedule in processed)
            {
                result.Add(schedule.GetInfo());
            }

            return Ok(result);
        }
    }

    public class CreateFeedingScheduleDto
    {
        public Guid AnimalId { get; set; }
        public DateTime FeedingTime { get; set; }
        public string FoodType { get; set; }
    }

    public class UpdateFeedingScheduleDto
    {
        public DateTime NewFeedingTime { get; set; }
        public string NewFoodType { get; set; }
    }
}