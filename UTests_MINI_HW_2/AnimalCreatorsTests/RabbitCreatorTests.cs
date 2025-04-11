using System;
using System.IO;
using Xunit;
using MINI_HW_2.AnimalCreators;
using MINI_HW_2.Domain.Animals;

namespace UTests_MINI_HW_2.AnimalCreatorsTests
{
    [Collection("Sequential Console Tests")]
    public class RabbitCreatorTests
    {
        [Fact]
        public void CreateAnimal_ReturnsRabbit_WhenValidInputProvided()
        {
            string inputSequence = "Bunny" + Environment.NewLine +
                                   "3" + Environment.NewLine +
                                   "8" + Environment.NewLine;
            using (var sr = new StringReader(inputSequence))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new RabbitCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.NotNull(animal);
                Assert.IsType<Rabbit>(animal);
                Assert.Equal("Bunny", animal!.Name);
            }
        }

        [Fact]
        public void CreateAnimal_ReturnsNull_WhenCanceledAtNameInput()
        {
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new RabbitCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.Null(animal);
            }
        }
    }
}