namespace Timesheet.Components.Models
{
    //I added this class so that I can return a message if theres a duplicate entry instead of null or an exception
    public class AddEntryResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public TimesheetEntry? Entry { get; set; }
    }
}
