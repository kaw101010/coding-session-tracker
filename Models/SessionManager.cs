using System.Data.SQLite;

namespace coding_tracker.Models
{
    public class SessionManager
    {
        public static void ToggleCodingSession(DatabaseConnector dbConnector, bool isUserCoding)
        {
            if (isUserCoding) {
                // end current user session
                DateTime EndSessionDateTime = DateTime.Now;
            }
            else {
                // start a user sesh
                DateTime currDateTime = DateTime.Now;
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