using coding_tracker.Models;

class Program
{
    private static readonly TrackerController _controller = new();
    public static void Main(string[] args)
    {
        _controller.RenderTracker();
    }
}