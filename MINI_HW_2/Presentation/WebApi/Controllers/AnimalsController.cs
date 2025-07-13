using Microsoft.AspNetCore.Mvc;
using MINI_HW_2.Application.Services;
using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Infrastructure.Repositories;

namespace MINI_HW_2.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly AnimalTransferService _transferService;

        public AnimalsController(IAnimalRepository animalRepository, AnimalTransferService transferService)
        {
            _animalRepository = animalRepository;
            _transferService = transferService;
        }

        // GET: api/animals
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAnimals()
        {
            var animals = _animalRepository.GetAll();
            var result = new List<object>();
            foreach (var animal in animals)
            {
                result.Add(animal.GetInfo());
            }

            return Ok(result);
        }

        // GET: api/animals/{id}
        [HttpGet("{id}")]
        public ActionResult<object> GetAnimal(Guid id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
                return NotFound();
            return Ok(animal.GetInfo());
        }

        // POST: api/animals
        [HttpPost]
        public ActionResult<object> CreateAnimal([FromBody] CreateAnimalDto dto)
        {
            Animal newAnimal = null;
            switch (dto.Species.ToLower())
            {
                case "rabbit":
                    newAnimal = new Rabbit(dto.Name, dto.BirthDate, dto.Sex, dto.FavoriteFood,
                        dto.Kindness.HasValue ? dto.Kindness.Value : 5);
                    break;
                case "tiger":
                    newAnimal = new Tiger(dto.Name, dto.BirthDate, dto.Sex, dto.FavoriteFood);
                    break;
                case "wolf":
                    newAnimal = new Wolf(dto.Name, dto.BirthDate, dto.Sex, dto.FavoriteFood);
                    break;
                case "monkey":
                    newAnimal = new Monkey(dto.Name, dto.BirthDate, dto.Sex, dto.FavoriteFood);
                    break;
                default:
                    return BadRequest("Неподдерживаемый вид животного.");
            }

            _animalRepository.Add(newAnimal);
            return CreatedAtAction(nameof(GetAnimal), new { id = newAnimal.Id }, newAnimal.GetInfo());
        }

        // DELETE: api/animals/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(Guid id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
                return NotFound();
            _animalRepository.Delete(id);
            return NoContent();
        }

        // PUT: api/animals/{id}/move
        [HttpPut("{id}/move")]
        public IActionResult MoveAnimal(Guid id, [FromBody] MoveAnimalDto dto)
        {
            try
            {
                _transferService.TransferAnimal(id, dto.TargetEnclosureId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }

    public class CreateAnimalDto
    {
        public string Species { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public Sex Sex { get; set; }
        public string FavoriteFood { get; set; }
        public int? Kindness { get; set; }
    }

    public class MoveAnimalDto
    {
        public Guid TargetEnclosureId { get; set; }
    }
}