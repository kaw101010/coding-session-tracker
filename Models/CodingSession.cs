namespace coding_tracker.Models {
    public class CodingSession
    {
        public int Id { get; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        private static int nextId = 111;

        public CodingSession() {
            this.Id = ++nextId;
            this.StartTime = default;
            this.EndTime = default;
        }

        public CodingSession(DateTime startTime, DateTime endTime)
        {
            this.Id = ++nextId;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public TimeSpan CodingDuration() {
            return this.EndTime - this.StartTime;
        }
    }
}