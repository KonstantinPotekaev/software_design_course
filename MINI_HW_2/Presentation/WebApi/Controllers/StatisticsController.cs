using Microsoft.AspNetCore.Mvc;
using MINI_HW_2.Application.Services;

namespace MINI_HW_2.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ZooStatisticsService _statsService;

        public StatisticsController(ZooStatisticsService statsService)
        {
            _statsService = statsService;
        }

        // GET: api/statistics
        [HttpGet]
        public IActionResult GetStatistics()
        {
            var totalAnimals = _statsService.GetTotalAnimals();
            var animalsBySpecies = _statsService.GetAnimalsBySpecies();
            var (healthy, sick) = _statsService.GetHealthStats();
            var totalEnclosures = _statsService.GetTotalEnclosures();
            var enclosuresOccupancy = _statsService.GetEnclosuresOccupancy();

            var result = new
            {
                TotalAnimals = totalAnimals,
                AnimalsBySpecies = animalsBySpecies,
                HealthStats = new { Healthy = healthy, Sick = sick },
                TotalEnclosures = totalEnclosures,
                EnclosuresOccupancy = enclosuresOccupancy
            };

            return Ok(result);
        }
    }
}