namespace Timesheet.Components.Models
{
    public class WeeklyUserEntriesResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<TimesheetEntry> Entries { get; set; } = new();
    }
}
