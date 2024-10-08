using coding_tracker.Controllers;
using coding_tracker.Models;
using coding_tracker.Validators;
using Spectre.Console;

namespace coding_tracker.Managers
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
                    DataVisualController.DisplayMessage("Invalid Date Format!");
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
                    DataVisualController.DisplayMessage("Invalid Time Format!");
                }
            }
        }

        public static int PromptIdOfSession(List<CodingSession> sessions, bool updateIfTrueElseDelete = true) {
            while (true) {
                string? sessionID = AnsiConsole.Prompt<string>(new TextPrompt<string>(
                                            @"Enter the [green]ID[/] of the coding session you want to " +
                                            $"{(updateIfTrueElseDelete ? "update" : "delete")}: ")
                                            .AllowEmpty());
                if (UserInputValidator.IsSessionIdValid(sessionID)) {
                    int intSessionID = int.Parse(sessionID);
                    IEnumerable<int> SearchQuery = UserInputValidator.IdInSessionsList(sessions, intSessionID);
                    if (SearchQuery.Any() && sessions.Count >= 1) {
                        return SearchQuery.Single();
                    }
                    else if (sessions.Count == 1 && !SearchQuery.Any()) {
                        DataVisualController.DisplayMessage("[red]No complete coding sessions have been logged[/]");
                        return -1;
                    }
                    else {
                        DataVisualController.DisplayMessage("[red]Session ID does not exist or session is still running![/]");
                    }
                }
                else {
                    DataVisualController.DisplayMessage("Invalid Session ID!");
                }
            }
        }
    }
}