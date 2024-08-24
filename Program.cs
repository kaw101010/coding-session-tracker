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
            DataVisualController.DisplayMessage("Error connecting to database...Please try again");
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
                    .Title("\n[green bold]What do you want to do?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(choices));

            if (choice != "Quit") {
                DataVisualController.RenderSelectedChoice(choice);
            }
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
                    DataVisualController.RenderSelectedChoice("GoodBye :)");
                    flag = true;
                    break;
            }
        }
        databaseConnector.CloseConnection();
    }
}