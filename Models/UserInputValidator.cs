using Spectre.Console;

namespace coding_tracker.Models
{
    class UserInputValidator
    {
        public static bool IsValidInputDate(string? date)
        {
            return DateOnly.TryParse(date, out DateOnly d) && d <= DateOnly.FromDateTime(DateTime.Now);
        }

        public static bool IsValidInputTime(string? time)
        {
            return TimeOnly.TryParse(time, out TimeOnly t) && t <= TimeOnly.FromDateTime(DateTime.Now);
        }

        public static bool IsValidInputDateAndTime(DateOnly currDate, TimeOnly currTime)
        {
            DateTime d = DateTime.Parse($"{currDate} {currTime}");
            return d <= DateTime.Now;
        }

        public static void DisplayMessage(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
            return;
        }

    }
}