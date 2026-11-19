using System.Globalization;

namespace ConsoleClient
{
    // Helper methods for reading and checking what the user types.
   
    public static class ConsoleInput
    {
        public static string ReadText(string label, string? current = null)
        {
            while (true)
            {
                Console.Write(current == null ? $"{label}: " : $"{label} [{current}]: ");
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                if (current != null)
                    return current;

                Console.WriteLine("The value cannot be empty.");
            }
        }

        public static int ReadInt(string label, int min, int max, int? current = null)
        {
            while (true)
            {
                Console.Write(current == null ? $"{label}: " : $"{label} [{current}]: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) && current != null)
                    return current.Value;
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Enter a whole number between {min} and {max}.");
            }
        }
        public static decimal ReadDecimal(string label, decimal min, decimal max, decimal? current = null)
        {
            while (true)
            {
                Console.Write(current == null ? $"{label}: " : $"{label} [{current:F2}]: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) && current != null)
                    return current.Value;

                // Both 999.50 and 999,50 are accepted
                if (input != null
                    && decimal.TryParse(input.Replace(',', '.'), NumberStyles.Number,
                        CultureInfo.InvariantCulture, out decimal value)
                    && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Enter a number between {min} and {max}.");
            }
        }
        public static DateOnly ReadDate(string label, DateOnly? current = null)
        {
            while (true)
            {
                string shown = current == null ? "" : $" [{current.Value:dd.MM.yyyy.}]";
                Console.Write($"{label} (dd.MM.yyyy.){shown}: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) && current != null)
                    return current.Value;
                if (DateOnly.TryParseExact(input, "dd.MM.yyyy.", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateOnly value))
                    return value;

                Console.WriteLine("Wrong format. Example: 12.05.1980.");
            }
        }
        public static string ReadEmail(string label, string? current = null)
        {
            while (true)
            {
                string value = ReadText(label, current);
                int at = value.IndexOf('@');

                // Needs text before @, a dot after @, no spaces, and must not end with a dot
                if (at > 0 && value.IndexOf('.', at) > at + 1 && !value.Contains(' ') && !value.EndsWith('.'))
                    return value;

                Console.WriteLine("Wrong e-mail format. Example: name@mail.com");
            }
        }

        public static string ReadPhone(string label, string? current = null)
        {
            while (true)
            {
                string value = ReadText(label, current);
                int digits = value.Count(char.IsDigit);
                bool onlyAllowed = value.All(c => char.IsDigit(c) || c == '+' || c == ' ' || c == '-' || c == '/');

                if (onlyAllowed && digits >= 6 && digits <= 15)
                    return value;

                Console.WriteLine("Wrong phone number. Use digits (and + - / or spaces). Example: 065 123 456");
            }
        }

        public static bool Confirm(string question)
        {
            Console.Write($"{question} (y/n): ");
            string? input = Console.ReadLine();
            return input != null && input.Trim().ToLower() == "y";
        }
    }
}