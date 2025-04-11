namespace MINI_HW_2.Utils
{
    public static class InputHelper
    {
        public static string? ReadNonEmptyString(string prompt, string cancelKeyword = "q")
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.Equals(input, cancelKeyword, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine(
                    $"Ввод не может быть пустым. Попробуйте снова или введите '{cancelKeyword}' для отмены.");
            }
        }

        public static int? ReadInt(string prompt, int? rangeStart = null, int? rangeEnd = null,
            string cancelKeyword = "q")
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (string.Equals(input, cancelKeyword, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                if (!int.TryParse(input, out int value))
                {
                    Console.WriteLine($"Ошибка: Введите корректное целое число или '{cancelKeyword}' для отмены.");
                    continue;
                }

                if ((rangeStart.HasValue && value < rangeStart.Value) || (rangeEnd.HasValue && value > rangeEnd.Value))
                {
                    Console.WriteLine(
                        $"Ошибка: Число должно быть в диапазоне [{rangeStart ?? int.MinValue}, {rangeEnd ?? int.MaxValue}]. Попробуйте снова.");
                    continue;
                }

                return value;
            }
        }


        public static string ReadNonEmptyStringStrict(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Ввод не может быть пустым. Попробуйте снова.");
            }
        }


        public static int ReadIntStrict(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                    return value;
                Console.WriteLine("Неверное число. Попробуйте снова.");
            }
        }
    }
}