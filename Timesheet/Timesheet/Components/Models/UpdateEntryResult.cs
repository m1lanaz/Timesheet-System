namespace Timesheet.Components.Models
{
    public class UpdateEntryResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public TimesheetEntry? Entry { get; set; }
    }
}
