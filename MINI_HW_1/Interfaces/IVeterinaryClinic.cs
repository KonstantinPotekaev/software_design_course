using MINI_HW_1.Domain.Animals;

namespace MINI_HW_1.Interfaces
{
    /// <summary>
    /// Интерфейс для ветеринарной клиники, осуществляющей проверку животных.
    /// </summary>
    public interface IVeterinaryClinic
    {
        bool CheckAnimal(Animal animal);
    }
}