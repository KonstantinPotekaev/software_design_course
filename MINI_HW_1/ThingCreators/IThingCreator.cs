using MINI_HW_1.Domain.Things;

namespace MINI_HW_1.ThingCreators
{
    public interface IThingCreator
    {
        string ThingTypeName { get; }
        Thing CreateThing();
    }
}