using coding_tracker.Controllers;

class Program
{
    private static readonly TrackerController _controller = new();
    public static void Main(string[] args)
    {
        _controller.RenderTracker();
    }
}