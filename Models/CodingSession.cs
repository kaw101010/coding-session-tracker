namespace coding_tracker.Models {
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Comments { get; set; }
        private static int nextId = 111;

        public CodingSession() {
            this.Id = ++nextId;
            this.StartTime = DateTime.Now;
            this.EndTime = null;
            this.Duration = null;
            this.Comments = null;
        }

        public CodingSession(DateTime startTime, DateTime endTime)
        {
            this.Id = ++nextId;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Duration = GetTimeSpan(startTime, endTime);
        }

        public static TimeSpan GetTimeSpan(DateTime start, DateTime end) {
            return end - start;
        }
    }
}