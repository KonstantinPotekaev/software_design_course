using System;
using System.Collections.Generic;
using System.Linq;
using MINI_HW_2.Domain.Enclosures;

namespace MINI_HW_2.Infrastructure.Repositories
{
    public class InMemoryEnclosureRepository : IEnclosureRepository
    {
        private readonly List<Enclosure> _enclosures = new List<Enclosure>();

        public void Add(Enclosure enclosure) => _enclosures.Add(enclosure);

        public Enclosure GetById(Guid id) => _enclosures.FirstOrDefault(e => e.Id == id);

        public IEnumerable<Enclosure> GetAll() => _enclosures;

        public void Update(Enclosure enclosure)
        {
            var index = _enclosures.FindIndex(e => e.Id == enclosure.Id);
            if (index >= 0)
                _enclosures[index] = enclosure;
        }

        public void Delete(Guid id)
        {
            var enclosure = GetById(id);
            if (enclosure != null)
                _enclosures.Remove(enclosure);
        }
    }
}