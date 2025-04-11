using MINI_HW_2.Domain.Animals;
using MINI_HW_2.Domain.Things;
using MINI_HW_2.Interfaces;

namespace MINI_HW_2.Domain
{
    public class Zoo
    {
        private readonly IVeterinaryClinic _veterinaryClinic;
        private readonly List<Animal> _animals;
        private readonly List<Thing> _things;
        private int _nextInventoryNumber;

        public Zoo(IVeterinaryClinic veterinaryClinic)
        {
            _veterinaryClinic = veterinaryClinic;
            _animals = new List<Animal>();
            _things = new List<Thing>();
            _nextInventoryNumber = 1;
        }

        public bool AddAnimal(Animal animal)
        {
            if (_veterinaryClinic.CheckAnimal(animal))
            {
                animal.Number = _nextInventoryNumber++;
                _animals.Add(animal);
                return true;
            }

            return false;
        }

        public void AddThing(Thing thing)
        {
            thing.Number = _nextInventoryNumber++;
            _things.Add(thing);
        }

        public int GetTotalFoodConsumption() =>
            _animals.Sum(a => a.Food);

        public List<Herbo> GetContactZooCandidates() =>
            _animals.OfType<Herbo>().Where(h => h.Kindness > 5).ToList();

        public List<IInventory> GetAllInventoryItems()
        {
            var items = new List<IInventory>();
            items.AddRange(_animals);
            items.AddRange(_things);
            return items;
        }

        public List<Animal> GetAnimals() => _animals;
        public List<Thing> GetThings() => _things;
    }
}