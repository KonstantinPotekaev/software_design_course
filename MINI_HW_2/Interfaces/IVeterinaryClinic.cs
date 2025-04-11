using MINI_HW_2.Domain.Animals;

namespace MINI_HW_2.Interfaces
{
    /// <summary>
    /// Интерфейс для ветеринарной клиники, осуществляющей проверку животных.
    /// </summary>
    public interface IVeterinaryClinic
    {
        bool CheckAnimal(Animal animal);
    }
}