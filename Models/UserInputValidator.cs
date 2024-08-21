using Spectre.Console;

namespace coding_tracker.Models
{
    class UserInputValidator
    {
        public static bool IsValidInputDate(string? date)
        {
            return DateOnly.TryParse(date, out DateOnly d);
        }

        public static void DisplayMessage(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
            return;
        }

    }
}