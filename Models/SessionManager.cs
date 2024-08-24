namespace coding_tracker.Models
{
    public class SessionManager
    {
        public static bool IsUserCodingCurrently(DatabaseConnector dbConnector) {
            return dbConnector.HasIncompleteRecords();
        }

        public static void ToggleCodingSession(DatabaseConnector dbConnector, bool isUserCoding)
        {
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
                // simulate a waiting time of 250 ms
                var t = Task.Run(async delegate
                        {
                            await Task.Delay(500);
                            return 0;
                        });
                t.Wait();
                DataVisualController.DisplayMessage("[red]Session Complete![/]\n");
            }
            else {
                // start a user sesh
                DateTime currDateTime = DateTime.Now;
                dbConnector.InsertRecordIntoTable(currDateTime, null, null, null);
                DataVisualController.DisplayMessage("[red]Session Started, have fun coding![/]\n");
            }
        }

        public static void LogSession(DatabaseConnector dbConnector)
        {
            // prompt user for date, time, and comments abt session
            // store in db
            DateOnly dt = SessionPrompt.PromptDateFromUser();
            List<CodingSession> codeSessions = dbConnector.GetSessionsOnDate(dt);
            DataVisualController.VisualizeCodingSessionsInTable(codeSessions, dt);
            TimeOnly startTime = SessionPrompt.PromptTimeFromUser(promptStartTimeToggle: true, sessionDate: dt);
            DateTime fullStartTime =DateTime.Parse($"{dt} {startTime}");
            TimeOnly endTime = SessionPrompt.PromptTimeFromUser(promptStartTimeToggle: false, sessionDate: dt);
            // if end time < start time, user worked overnight after 12 so add date by 1
            DateTime fullEndTime = DateTime.Parse($"{(endTime < startTime ? dt.AddDays(1) : dt)} {endTime}");
            string comments = SessionPrompt.PromptCommentsFromUser();
            dbConnector.InsertRecordIntoTable(fullStartTime, fullEndTime, 
                                                CodingSession.GetTimeSpan(fullStartTime, fullEndTime),
                                                comments);
            DataVisualController.DisplayMessage("[red]Session Logged! Well done![/]\n");
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
            DateOnly dt = SessionPrompt.PromptDateFromUser();
            List<CodingSession> codeSessions = dbConnector.GetSessionsOnDate(dt);
            DataVisualController.VisualizeCodingSessionsInTable(codeSessions, dt);
            int IdOfSession = SessionPrompt.PromptIdOfSession(codeSessions, updateIfTrueElseDelete: true);
            // dbConnector.DeleteRecordFromTable(IdOfSession);
        }

        public static void ViewSession(DatabaseConnector dbConnector)
        {
            // visualize all coding sessions for a day in a table
            // use spectre
            DateOnly dt = SessionPrompt.PromptDateFromUser();
            List<CodingSession> codeSessions = dbConnector.GetSessionsOnDate(dt);
            DataVisualController.VisualizeCodingSessionsInTable(codeSessions, dt);
        }
    }
}