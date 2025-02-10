using System;
using System.IO;
using Xunit;
using MINI_HW_1.ThingCreators;
using MINI_HW_1.Domain.Things;

namespace UTests_MINI_HW_1.ThingCreatorsTests
{
    [Collection("Sequential Console Tests")]
    public class TableCreatorTests
    {
        [Fact]
        public void CreateThing_ReturnsTable_WhenValidInputProvided()
        {
            string inputSequence = "Dining Table" + Environment.NewLine;
            using (var sr = new StringReader(inputSequence))
            {
                Console.SetIn(sr);
                IThingCreator creator = new TableCreator();
                Thing? thing = creator.CreateThing();
                Assert.NotNull(thing);
                Assert.IsType<Table>(thing);
                Assert.Equal("Dining Table", thing!.Name);
            }
        }

        [Fact]
        public void CreateThing_ReturnsNull_WhenCanceledAtNameInput()
        {
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                IThingCreator creator = new TableCreator();
                Thing? thing = creator.CreateThing();
                Assert.Null(thing);
            }
        }
    }
}