using System.Data.SQLite;
using System.Runtime.CompilerServices;
using Spectre.Console;

namespace coding_tracker.Models
{
    public class SessionManager
    {
        public static TimeSpan GetTimeSpan(DateTime d1, DateTime d2) {
            return d2 - d1;
        }

        public static bool IsUserCodingCurrently(DatabaseConnector dbConnector) {
            return dbConnector.HasIncompleteRecords();
        }

        public static void ToggleCodingSession(DatabaseConnector dbConnector, bool isUserCoding)
        {
            // simulate a waiting time of 250 ms
            var t = Task.Run(async delegate
                    {
                        await Task.Delay(250);
                        return 0;
                    });
            t.Wait();
            if (isUserCoding) {
                // end current user session
                DateTime EndSessionDateTime = DateTime.Now;
                string? comments = AnsiConsole.Prompt(new TextPrompt<string>("Add some [green]comments about this session[/]: "));
                var columnsToUpdate = new Dictionary<string, object>
                            {
                                { "END_TIME", EndSessionDateTime.ToString("HH:mm:ss") },
                                { "comment", comments }
                            };
                dbConnector.UpdateRecordAndEndSession(columnsToUpdate);
            }
            else {
                // start a user sesh
                DateTime currDateTime = DateTime.Now;
                dbConnector.InsertRecordIntoTable(currDateTime.ToString("dd-MM-yyyy"), currDateTime.ToString("HH:mm:ss"), null, null, null);
            }
        }

        public static void LogSession(DatabaseConnector dbConnector)
        {
            // Implementation here
        }

        public static void UpdateSession(DatabaseConnector dbConnector)
        {
            // Implementation here
        }

        public static void DeleteSession(DatabaseConnector dbConnector)
        {
            // Implementation here
        }
    }
}