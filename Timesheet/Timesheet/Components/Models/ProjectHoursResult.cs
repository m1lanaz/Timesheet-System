namespace Timesheet.Components.Models
{
    public class WeeklyProjectHoursResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<ProjectHoursResult> Results { get; set; } = new();
    }

    public class ProjectHoursResult
    {
        public int ProjectID { get; set; }
        public decimal TotalHours { get; set; }
    }
}
