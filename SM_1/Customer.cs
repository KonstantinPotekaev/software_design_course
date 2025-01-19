namespace SM_1;

public class Customer
{
    public string Name { get; }
    public Car? Car { get; private set; }

    public Customer(string name, Car? car)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name must not be empty.", nameof(name));

        Name = name;
        Car = car;
    }

    public void ChangeCar(Car car)
    {
        Car = car;
    }

    public override string ToString()
    {
        return Car != null ? $"Customer {Name}, {Car}" : $"Customer {Name}, no car";
    }
}