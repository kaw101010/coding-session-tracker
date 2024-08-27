using coding_tracker.Managers;
using coding_tracker.Models;
using coding_tracker.Repositories;

namespace coding_tracker.Controllers
{
    class TrackerController
    {
        private readonly SessionManager sessionManager = new();
        public void RenderTracker() {
            DataVisualController.DisplayHeading();
            var databaseConnector = new DatabaseConnector();
            databaseConnector.CreateTable();
            bool flag = false;
            bool codingNow = sessionManager.IsUserCodingCurrently(databaseConnector);
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
                string userChoice = DataVisualController.DisplayChoicesToUser(choices);

                if (userChoice != "Quit") {
                    DataVisualController.RenderSubheading(userChoice);
                }
                switch (choices.IndexOf(userChoice)) {
                    case (int)Options.StartOrEndSession:
                        sessionManager.ToggleCodingSession(databaseConnector, codingNow);
                        codingNow = !codingNow;
                        break;
                    case (int)Options.LogSession:
                        sessionManager.LogSession(databaseConnector);
                        break;
                    case (int)Options.UpdateSession:
                        sessionManager.UpdateSession(databaseConnector);
                        break;
                    case (int)Options.DeleteSession:
                        sessionManager.DeleteSession(databaseConnector);
                        break;
                    case (int)Options.ViewSession:
                        sessionManager.ViewSession(databaseConnector);
                        break;
                    case (int)Options.Quit:
                        DataVisualController.RenderSubheading("GoodBye :)");
                        flag = true;
                        break;
                }
            }
            databaseConnector.CloseConnection();
        }
    }
}