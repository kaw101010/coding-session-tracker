using Spectre.Console;

namespace coding_tracker.Models
{
    class SessionPrompt
    {
        public static DateOnly PromptDateFromUser()
        {
            while (true) {
                string? date = AnsiConsole.Prompt(new TextPrompt<string>(
                            "Enter the [green]date[/] of the coding session [green](yyyy-mm-dd)[/] (PRESS ENTER FOR TODAY): ")
                            .AllowEmpty());
                if (string.IsNullOrEmpty(date)) {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
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
            return userComments ?? "";
        }
    }
}