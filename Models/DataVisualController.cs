using Dapper;
using Spectre.Console;

namespace coding_tracker.Models
{
    class DataVisualController
    {
        public static void VisualizeCodingSessionsInTable(List<CodingSession> codingSessions, DateOnly sessionDate) {
            if (codingSessions.Count < 1) {
                var noCodeSessions = new Text($"No coding sessions on {sessionDate}\n\n");
                AnsiConsole.Write(noCodeSessions);
                return;
            }
            string _dateTimeFormat = "MM/dd/yyyy hh:mm tt";
            var table = new Table();
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
            orderedCodingSessions.AsList().ForEach((session) => {
                table.AddRow(
                                // show id in blue if session is still going on, else show
                                $"[{(session.EndTime != null ? "red" : "blue")}]{session.Id}[/]",
                                session.StartTime.ToString(_dateTimeFormat),
                                session.EndTime?.ToString(_dateTimeFormat) ?? "[blue]...Session hasn't ended[/]", 
                                session.Duration?.ToString("h\\:mm") ?? "[blue]...Session hasn't ended[/]",
                                (session.Comments ?? "[blue]N/A[/]").ToString());
            });
            AnsiConsole.Write(table.Centered());
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
    }
}