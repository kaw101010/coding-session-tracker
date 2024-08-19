using coding_tracker.Models;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Write(
                new FigletText("CODING TRACKER")
                .Centered()
                .Color(Color.Yellow));
        bool flag = false;
        bool codingNow = false;
        var databaseConnector = new DatabaseConnector();
        databaseConnector.OpenConnection();
        if (databaseConnector.DatabaseNotConnected()) {
            AnsiConsole.MarkupLine("[red]Error connecting to database...[/]");
            return;
        }
        databaseConnector.CreateTable();

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
                            .Title("What's your [green]favorite fruit[/]?")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
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
                case (int)Options.Quit:
                    flag = true;
                    var rule = new Rule("[yellow bold]Goodbye :)[/]");
                    rule.RuleStyle("yellow");
                    rule.Border(BoxBorder.Ascii);
                    AnsiConsole.Write(rule);
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Enter a valid option please[/]");
                    break;
            }
        }
        databaseConnector.CloseConnection();
    }
}