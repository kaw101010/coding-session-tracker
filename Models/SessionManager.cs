using System.Data.SQLite;
using System.Runtime.CompilerServices;
using Spectre.Console;

namespace coding_tracker.Models
{
    public class SessionManager
    {
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
                string comments = SessionPrompt.PromptCommentsFromUser();
                var columnsToUpdate = new Dictionary<string, object>
                            {
                                { "END_TIME", EndSessionDateTime },
                                { "comment", comments }
                            };
                dbConnector.UpdateRecordAndEndSession(columnsToUpdate);
                UserInputValidator.DisplayMessage("[red]Session Complete![/]\n");
            }
            else {
                // start a user sesh
                DateTime currDateTime = DateTime.Now;
                dbConnector.InsertRecordIntoTable(currDateTime.ToString(), null, null, null);
            }
        }

        public static void LogSession(DatabaseConnector dbConnector)
        {
            // prompt user for date, time, and comments abt session
            // store in db
            DateOnly dt = SessionPrompt.PromptDateFromUser();
            List<CodingSession> codeSessions = dbConnector.GetSessionsOnDate(dt);
            codeSessions.ForEach(x => System.Console.WriteLine(x.Duration));
            // visualize data in table
        }

        public static void UpdateSession(DatabaseConnector dbConnector)
        {
            // ask user for date, show all times and ask which time
            // update the value in db
        }

        public static void DeleteSession(DatabaseConnector dbConnector)
        {
            // ask user for date, show all times and ask which time, or times
            // delete all values in db
        }

        public static void ViewSession(DatabaseConnector dbConnector)
        {
            // visualize all coding sessions for a day in a table
            // use spectre
        }
    }
}