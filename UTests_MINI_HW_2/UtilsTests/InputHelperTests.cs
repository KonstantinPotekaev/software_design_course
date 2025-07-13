using MINI_HW_2.Utils;

namespace UTests_MINI_HW_2.UtilsTests
{
    [Collection("Sequential Console Tests")]
    public class InputHelperTests
    {
        [Fact]
        public void ReadNonEmptyString_ReturnsInput_WhenNotEmpty()
        {
            // Arrange
            var expected = "Hello";
            using (var sr = new StringReader(expected + Environment.NewLine))
            {
                Console.SetIn(sr);
                // Act
                var result = InputHelper.ReadNonEmptyString("Введите строку: ");
                // Assert
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void ReadNonEmptyString_ReturnsNull_WhenInputIsCancelKeyword()
        {
            // Arrange
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                // Act
                string? result = InputHelper.ReadNonEmptyString("Введите строку: ");
                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public void ReadInt_ReturnsNumber_WhenValidInput()
        {
            // Arrange
            using (var sr = new StringReader("42" + Environment.NewLine))
            {
                Console.SetIn(sr);
                // Act
                int? result = InputHelper.ReadInt("Введите число: ");
                // Assert
                Assert.Equal(42, result);
            }
        }

        [Fact]
        public void ReadInt_ReturnsNull_WhenInputIsCancelKeyword()
        {
            // Arrange
            using (var sr = new StringReader("q" + Environment.NewLine))
            {
                Console.SetIn(sr);
                // Act
                int? result = InputHelper.ReadInt("Введите число: ");
                // Assert
                Assert.Null(result);
            }
        }
    }
}