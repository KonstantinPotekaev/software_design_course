using System;
using System.IO;
using Xunit;
using MINI_HW_2.AnimalCreators;
using MINI_HW_2.Domain.Animals;

namespace UTests_MINI_HW_2.AnimalCreatorsTests
{
    [Collection("Sequential Console Tests")]
    public class WolfCreatorTests
    {
        [Fact]
        public void CreateAnimal_ReturnsWolf_WhenValidInputProvided()
        {
            string inputSequence = "Alpha" + Environment.NewLine +
                                   "10" + Environment.NewLine;
            using (var sr = new StringReader(inputSequence))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new WolfCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.NotNull(animal);
                Assert.IsType<Wolf>(animal);
                Assert.Equal("Alpha", animal!.Name);
            }
        }

        [Fact]
        public void CreateAnimal_ReturnsNull_WhenCanceledAtNameInput()
        {
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new WolfCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.Null(animal);
            }
        }
    }
}