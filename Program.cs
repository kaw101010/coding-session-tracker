using coding_tracker.Models;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        DataVisualController.DisplayHeading();
        var databaseConnector = new DatabaseConnector();
        databaseConnector.CreateTable();
        bool flag = false;
        bool codingNow = SessionManager.IsUserCodingCurrently(databaseConnector);
        databaseConnector.OpenConnection();
        if (databaseConnector.DatabaseNotConnected()) {
            UserInputValidator.DisplayMessage("Error connecting to database...");
            return;
        }

        while (!flag) {
            List<string> choices = [
                !codingNow ? "Start a new coding session" : "End current coding session",
                "Log a coding session",
                "Update a coding session",
                "Delete a coding session",
                "View all coding session for a day",
                "Quit"];
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green bold]What do you want to do?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(choices));
                        
            switch (choices.IndexOf(choice)) {
                case (int)Options.StartOrEndSession:
                    SessionManager.ToggleCodingSession(databaseConnector, codingNow);
                    codingNow = !codingNow;
                    break;
                case (int)Options.LogSession:
                    SessionManager.LogSession(databaseConnector);
                    break;
                case (int)Options.UpdateSession:
                    SessionManager.UpdateSession(databaseConnector);
                    break;
                case (int)Options.DeleteSession:
                    SessionManager.DeleteSession(databaseConnector);
                    break;
                case (int)Options.ViewSession:
                    SessionManager.ViewSession(databaseConnector);
                    break;
                case (int)Options.Quit:
                    flag = true;
                    var rule = new Rule("[yellow bold]Goodbye :)[/]");
                    rule.RuleStyle("yellow");
                    rule.Border(BoxBorder.Ascii);
                    AnsiConsole.Write(rule);
                    break;
                default:
                    UserInputValidator.DisplayMessage("Enter a valid option please!");
                    break;
            }
        }
        databaseConnector.CloseConnection();
    }
}