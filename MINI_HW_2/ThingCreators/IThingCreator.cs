using MINI_HW_2.Domain.Things;

namespace MINI_HW_2.ThingCreators
{
    public interface IThingCreator
    {
        string ThingTypeName { get; }
        Thing? CreateThing();
    }
}