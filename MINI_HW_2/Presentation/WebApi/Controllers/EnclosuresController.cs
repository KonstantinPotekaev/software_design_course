using Microsoft.AspNetCore.Mvc;
using MINI_HW_2.Domain.Enclosures;
using MINI_HW_2.Infrastructure.Repositories;

namespace MINI_HW_2.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosuresController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepository;

        public EnclosuresController(IEnclosureRepository enclosureRepository)
        {
            _enclosureRepository = enclosureRepository;
        }

        // GET: api/enclosures
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetEnclosures()
        {
            var enclosures = _enclosureRepository.GetAll();
            var result = enclosures.Select(enclosure => enclosure.GetInfo()).ToList();

            return Ok(result);
        }

        // GET: api/enclosures/{id}
        [HttpGet("{id}")]
        public ActionResult<object> GetEnclosure(Guid id)
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();
            return Ok(enclosure.GetInfo());
        }

        // POST: api/enclosures
        [HttpPost]
        public ActionResult<object> CreateEnclosure([FromBody] CreateEnclosureDto dto)
        {
            var enclosure = new Enclosure(dto.Type, dto.Size, dto.Capacity);
            _enclosureRepository.Add(enclosure);
            return CreatedAtAction(nameof(GetEnclosure), new { id = enclosure.Id }, enclosure.GetInfo());
        }

        // DELETE: api/enclosures/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEnclosure(Guid id)
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();
            _enclosureRepository.Delete(id);
            return NoContent();
        }

        // POST: api/enclosures/{id}/clean
        [HttpPost("{id}/clean")]
        public IActionResult CleanEnclosure(Guid id)
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();
            enclosure.Clean();
            _enclosureRepository.Update(enclosure);
            return Ok(new
                { message = "Вольер убран", enclosureId = enclosure.Id, lastCleaned = enclosure.LastCleaned });
        }
    }

    public class CreateEnclosureDto
    {
        public EnclosureType Type { get; set; }
        public double Size { get; set; }
        public int Capacity { get; set; }
    }
}