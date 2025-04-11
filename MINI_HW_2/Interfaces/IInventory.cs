namespace MINI_HW_2.Interfaces
{
    /// <summary>
    /// Интерфейс для объектов, подлежащих инвентаризации.
    /// </summary>
    public interface IInventory
    {
        int Number { get; set; }
        string Name { get; }
    }
}