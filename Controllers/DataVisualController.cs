using coding_tracker.Models;
using Spectre.Console;

namespace coding_tracker.Controllers
{
    class DataVisualController
    {
        public static void VisualizeCodingSessionsInTable(List<CodingSession> codingSessions, DateOnly sessionDate) {
            if (codingSessions.Count < 1) {
                var noCodeSessions = new Text($"\nNo coding sessions on {sessionDate}\n",
                            style: new Style(Color.Aqua,background: Color.Blue,decoration: Decoration.Bold))
                            .Centered();
                AnsiConsole.Write(noCodeSessions);
                return;
            }
            string _dateTimeFormat = "MM/dd/yyyy hh:mm tt";
            var table = new Table();
            table.Title($"\n[bold yellow]Code Sessions on {sessionDate}[/]", 
                        style: new Style(Color.Aqua,background: Color.Blue))
                        .Centered();
            table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Start Time[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]End Time[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Duration[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Comments[/]").Centered());
            
            // sort data in descending for better understanding
            IEnumerable<CodingSession> orderedCodingSessions = 
                    from session in codingSessions
                    orderby session.Duration descending
                    select session;
            orderedCodingSessions.ToList().ForEach((session) => {
                table.AddRow(
                                // show id in blue if session is still going on, else show
                                $"[{(session.EndTime != null ? "red" : "blue")}]{session.Id}[/]",
                                $"[{(session.EndTime != null ? "":"blue")}]{session.StartTime.ToString(_dateTimeFormat)}[/]",
                                session.EndTime?.ToString(_dateTimeFormat) ?? "[blue]...Session hasn't ended[/]", 
                                session.Duration?.ToString("h\\:mm") ?? "[blue]...Session hasn't ended[/]",
                                (session.Comments ?? "[blue]N/A[/]").ToString());
            });
            AnsiConsole.Write(table.Centered());
        }

        public static void RenderSubheading(string selectedChoice)
        {
            var rule = new Rule($"\n[yellow bold]{selectedChoice}[/]\n");
            rule.RuleStyle(new Style(Color.Yellow, background: Color.Green, decoration: Decoration.Invert));
            rule.Border(BoxBorder.Ascii);
            AnsiConsole.Write(rule);
        }

        public static void DisplayHeading()
        {
            var panel = new Panel(new FigletText("CODING TRACKER")
                                .Centered()
                                .Color(Color.Yellow));
            panel.Border(BoxBorder.Double);
            panel.BorderColor(Color.Yellow);
            AnsiConsole.Write(panel);
        }

        public static string DisplayChoicesToUser(List<string> choices) {
            string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("\n[green bold]What do you want to do?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(choices));
            return choice;
        }

        public static void DisplayMessage(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
            return;
        }
    }
}