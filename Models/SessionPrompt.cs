using System.ComponentModel;
using Spectre.Console;

namespace coding_tracker.Models
{
    class SessionPrompt
    {
        public static DateOnly PromptDateFromUser()
        {
            while (true) {
                string date = AnsiConsole.Prompt(new TextPrompt<string>(
                            "Enter the [green]date[/] of the coding session [green](yyyy-mm-dd)[/] (Press ENTER for today): ")
                            .AllowEmpty());
                if (string.IsNullOrEmpty(date)) {
                    date = DateOnly.FromDateTime(DateTime.Now).ToString();
                }
                if (UserInputValidator.IsValidInputDate(date)) {
                    return DateOnly.Parse(date);
                }
                else {
                    UserInputValidator.DisplayMessage("Invalid Date Format!");
                }
            }
        }

        public static string PromptCommentsFromUser() {
            string? userComments = AnsiConsole.Prompt(new TextPrompt<string>(
                    "Add some [green]comments about this session[/]: "));
            return userComments ?? "--None--";
        }

        public static TimeOnly PromptTimeFromUser(DateOnly sessionDate, bool promptStartTimeToggle = true) {
            string timing = promptStartTimeToggle ? "start" : "end";
            while (true) {
                string? start_time = AnsiConsole.Prompt(new TextPrompt<string>(
                            $"Enter the [green]{timing} time[/] of the coding session [green](hh:mm in 24 hour format)[/]: ")
                            .AllowEmpty());
                if (UserInputValidator.IsValidInputTime(start_time) && 
                    UserInputValidator.IsValidInputDateAndTime(sessionDate, TimeOnly.Parse(start_time))) {
                    return TimeOnly.Parse(start_time);
                }
                else {
                    UserInputValidator.DisplayMessage("Invalid Time Format!");
                }
            }
        }
    }
}