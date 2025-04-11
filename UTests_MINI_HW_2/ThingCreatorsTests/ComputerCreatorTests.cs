using MINI_HW_2.ThingCreators;
using MINI_HW_2.Domain.Things;

namespace UTests_MINI_HW_2.ThingCreatorsTests
{
    [Collection("Sequential Console Tests")]
    public class ComputerCreatorTests
    {
        [Fact]
        public void CreateThing_ReturnsComputer_WhenValidInputProvided()
        {
            string inputSequence = "Dell Inspiron" + Environment.NewLine;
            using (var sr = new StringReader(inputSequence))
            {
                Console.SetIn(sr);
                IThingCreator creator = new ComputerCreator();
                Thing? thing = creator.CreateThing();
                Assert.NotNull(thing);
                Assert.IsType<Computer>(thing);
                Assert.Equal("Dell Inspiron", thing!.Name);
            }
        }

        [Fact]
        public void CreateThing_ReturnsNull_WhenCanceledAtNameInput()
        {
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                IThingCreator creator = new ComputerCreator();
                Thing? thing = creator.CreateThing();
                Assert.Null(thing);
            }
        }
    }
}