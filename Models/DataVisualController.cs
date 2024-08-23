using System.ComponentModel.DataAnnotations;
using Spectre.Console;

namespace coding_tracker.Models
{
    class DataVisualController
    {
        public static void VisualizeCodingSessionsInTable(List<CodingSession> codingSessions) {
            if (codingSessions.Count < 1) {
                var noCodeSessions = new Text("No coding sessions on {}");
                AnsiConsole.Write(noCodeSessions);
            }
            var table = new Table();
            table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Start Time[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]End Time[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Duration[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Session Comments[/]").Centered());
            codingSessions.ForEach((session) => {
                table.AddRow(session.Id.ToString(), session.StartTime.ToString(),
                                session.EndTime?.ToString()! ?? "...Session hasn't ended", 
                                session.Duration?.ToString() ?? "...Session hasn't ended",
                                (session.Comments ?? "N/A").ToString());
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