namespace Helpers
{
    public static class Validations
    {
        public static string ValidateContent(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️ Invalid input. Please do not leave the field empty.");
                }
                else
                {
                    return input;
                }
            }
        }

        public static int ValidateNumber(string message)
        {
            while (true)
            {
                int number;
                Console.Write(message);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("⚠️ Invalid input. Please enter an integer number.");
                }
            }
        }

        public static double ValidateDouble(string message)
        {
            while (true)
            {
                double number;
                Console.Write(message);
                string? input = Console.ReadLine();
                if (double.TryParse(input, out number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("⚠️ Invalid input. Please enter a floating-point number.");
                }
            }
        }
    }
}