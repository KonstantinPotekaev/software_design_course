namespace SM_1;

public class Car
{
    public int SerialNumber { get; }
    public Engine Engine { get; }

    public Car(int serialNumber, int pedalSize)
    {
        Engine = new Engine(pedalSize);
        SerialNumber = serialNumber;
    }

    public override string ToString()
    {
        return $"Car {SerialNumber}, {Engine}";
    }
}