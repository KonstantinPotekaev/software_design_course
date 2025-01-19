namespace SM_1;

public class Engine
{
    public int PedalSize { get; set; }

    public Engine(int pedalSize)
    {
        PedalSize = pedalSize;
    }

    public override string ToString()
    {
        return $"Pedal Size: {PedalSize}";
    }
}