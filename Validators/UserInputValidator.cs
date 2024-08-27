using coding_tracker.Models;
using Spectre.Console;

namespace coding_tracker.Validators
{
    class UserInputValidator
    {
        public static bool IsValidInputDate(string? date)
        {
            return DateOnly.TryParse(date, out DateOnly d) && d <= DateOnly.FromDateTime(DateTime.Now);
        }

        public static bool IsValidInputTime(string? time)
        {
            return TimeOnly.TryParse(time, out TimeOnly t);
        }

        public static bool IsValidInputDateAndTime(DateOnly currDate, TimeOnly currTime)
        {
            DateTime d = DateTime.Parse($"{currDate} {currTime}");
            return d <= DateTime.Now;
        }

        public static bool IsSessionIdValid(string? id)
        {
            return int.TryParse(id, out int sessionID);
        }

        public static IEnumerable<int> IdInSessionsList(List<CodingSession> sessions, int idToSearch)
        {
            IEnumerable<int> intSearchQuery = 
                    from s in sessions
                    where s.Id == idToSearch && s.Duration != null
                    select s.Id;
            return intSearchQuery;
        }

    }
}