using Xunit;
using MINI_HW_2.AnimalCreators;
using MINI_HW_2.Domain.Animals;

namespace UTests_MINI_HW_2.AnimalCreatorsTests
{   
    [Collection("Sequential Console Tests")]
    public class MonkeyCreatorTests
    {
        [Fact]
        public void CreateAnimal_ReturnsMonkey_WhenValidInputProvided()
        {
            string inputSequence = "George" + Environment.NewLine +
                                   "5" + Environment.NewLine;
            using (var sr = new StringReader(inputSequence))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new MonkeyCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.NotNull(animal);
                Assert.IsType<Monkey>(animal);
                Assert.Equal("George", animal!.Name);
            }
        }

        [Fact]
        public void CreateAnimal_ReturnsNull_WhenCanceledAtNameInput()
        {
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new MonkeyCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.Null(animal);
            }
        }
    }
}