using System;
using System.Collections.Generic;
using MINI_HW_2.Domain.Enclosures;

namespace MINI_HW_2.Infrastructure.Repositories
{
    public interface IEnclosureRepository
    {
        void Add(Enclosure enclosure);
        Enclosure? GetById(Guid id);
        IEnumerable<Enclosure> GetAll();
        void Update(Enclosure enclosure);
        void Delete(Guid id);
    }
}