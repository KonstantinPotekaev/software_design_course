namespace SM_1;

public class FactoryAF
{
    private static readonly Random Random = new();

    private readonly List<Car> _cars = [];
    private readonly List<Customer> _customers;

    private static int _nextSerialNumber = 1;
    private static readonly (int, int) AvailablePedalSizeRange = (1, 10);

    public FactoryAF(List<Customer>? customers)
    {
        _customers = customers ?? new List<Customer>();
    }

    public void ProduceCars(int number = 1)
    {
        _cars.AddRange(Enumerable.Range(0, number).Select(_ => new Car(GenerateSerialNumber(), GeneratePedalSize())));
    }

    public void SaleCar()
    {
        foreach (var customer in _customers)
        {
            if (!_cars.Any()) continue;
            // Считаю, что ответственность за присваивание машины должна лежать на customer, поэтому написал доп метод.
            customer.ChangeCar(_cars.Last());
            _cars.RemoveAt(_cars.Count - 1);
        }

        if (!_cars.Any()) return;
        _cars.Clear();
    }

    private int GenerateSerialNumber()
    {
        return _nextSerialNumber++;
    }

    private int GeneratePedalSize()
    {
        return Random.Next(AvailablePedalSizeRange.Item1, AvailablePedalSizeRange.Item2);
    }

    public void ShowCars()
    {
        Console.WriteLine("Cars in stock:");
        if (_cars.Any())
            _cars.ForEach(Console.WriteLine);
        else
            Console.WriteLine("No cars available.");
    }

    public void ShowCustomers()
    {
        Console.WriteLine("Customers:");
        _customers.ForEach(Console.WriteLine);
    }
}