using System;
using MINI_HW_2.Interfaces;

namespace MINI_HW_2.Domain.Animals
{
    public abstract class Animal : IAlive, IInventory
    {
        public Guid Id { get; private set; }
        public string Species { get; protected set; }
        public string Name { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public Sex Sex { get; protected set; }
        public string FavoriteFood { get; protected set; }
        public HealthStatus HealthStatus { get; protected set; }
        public Guid? EnclosureId { get; private set; }
        public int Food { get; protected set; }
        public int Number { get; set; }

        protected Animal(string species, string name, DateTime birthDate, Sex sex, string favoriteFood, int food,
            HealthStatus healthStatus = HealthStatus.Healthy)
        {
            Id = Guid.NewGuid();
            Species = species;
            Name = name;
            BirthDate = birthDate;
            Sex = sex;
            FavoriteFood = favoriteFood;
            Food = food;
            HealthStatus = healthStatus;
        }

        public virtual void Feed()
        {
        }

        public virtual void Treat()
        {
            if (HealthStatus == HealthStatus.Sick)
                HealthStatus = HealthStatus.Healthy;
        }

        public virtual void MoveToEnclosure(Guid targetEnclosureId)
        {
            EnclosureId = targetEnclosureId;
        }

        public virtual object GetInfo() => new
        {
            Id,
            Species,
            Name,
            BirthDate,
            Sex,
            FavoriteFood,
            Food,
            HealthStatus,
            EnclosureId,
            Number
        };
    }
}