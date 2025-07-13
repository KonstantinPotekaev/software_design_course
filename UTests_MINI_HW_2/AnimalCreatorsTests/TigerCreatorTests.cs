using Xunit;
using MINI_HW_2.AnimalCreators;
using MINI_HW_2.Domain.Animals;

namespace UTests_MINI_HW_2.AnimalCreatorsTests
{
    [Collection("Sequential Console Tests")]
    public class TigerCreatorTests
    {
        [Fact]
        public void CreateAnimal_ReturnsTiger_WhenValidInputProvided()
        {
            string inputSequence = "Stripes" + Environment.NewLine +
                                   "15" + Environment.NewLine;
            using (var sr = new StringReader(inputSequence))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new TigerCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.NotNull(animal);
                Assert.IsType<Tiger>(animal);
                Assert.Equal("Stripes", animal!.Name);
            }
        }

        [Fact]
        public void CreateAnimal_ReturnsNull_WhenCanceledAtNameInput()
        {
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                IAnimalCreator creator = new TigerCreator();
                Animal? animal = creator.CreateAnimal();
                Assert.Null(animal);
            }
        }
    }
}