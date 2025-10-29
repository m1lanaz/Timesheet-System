namespace Timesheet.Components.Models
{
    public class TimesheetEntry
    {
        public Guid ID { get; set; } = Guid.NewGuid();//PK 
        public int UserID { get; set; } //FK
        public int ProjectID { get; set; } //FK
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public string? Description { get; set; }
    }
}
