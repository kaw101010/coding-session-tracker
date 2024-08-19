using coding_tracker.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Spectre.Console.Rendering;

class Program
{
    static void Main(string[] args)
    {
        bool flag = false;
        bool codingNow = false;
        var databaseConnector = new DatabaseConnector();
        if (databaseConnector.Connection.State != System.Data.ConnectionState.Open) {
            AnsiConsole.MarkupLine("[red]Error connecting to database...[/]");
            return;
        }
        var conn = databaseConnector.Connection;
        while (!flag) {
            AnsiConsole.Write(
                new FigletText("CODING TRACKER")
                    .Centered()
                    .Color(Color.Red));
            AnsiConsole.MarkupLine(!codingNow ? "(s) Start a coding session" : "(e) End current coding session");
            Console.WriteLine("(a) Log a coding session");
            Console.WriteLine("(u) Update a coding session");
            Console.WriteLine("(d) Delete a coding session");
            Console.WriteLine("(v) View a coding session for a day");
            Console.WriteLine("(q) Quit");
            Console.Write("Enter an option: ");
            string? choice = Console.ReadLine();
            switch (choice) {
                case "s":
                    SessionManager.ToggleCodingSession(conn);
                    codingNow = true;
                    break;
                case "e":
                    SessionManager.ToggleCodingSession(conn);
                    codingNow = false;
                    break;
                case "a":
                    SessionManager.LogSession(conn);
                    break;
                case "u":
                    SessionManager.UpdateSession(conn);
                    break;
                case "d":
                    SessionManager.DeleteSession(conn);
                    break;
                case "q":
                    flag = true;
                    AnsiConsole.MarkupLine($"Goodbye :)");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Enter a valid option please[/]");
                    break;
            }
        }
    }
}