using Timesheet.Components.Models;
using Timesheet.Components.Services;
using Xunit;

namespace Timesheet.Test.Services
{
    public class TimesheetGetAllEntries
    {

        private TimesheetService _service;

        public TimesheetGetAllEntries()
        {
            _service = new TimesheetService();
        }

        //Get all entries when empty
        [Fact]
        public void GetAllEntriesEmpty()
        {

            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Empty(allEntries);
        }

        //Get all entries after multiple entries added
        [Fact]
        public void GetAllEntries()
        {
            List<TimesheetEntry> entries = new List<TimesheetEntry>
            {
                new()
                {
                UserID = 123,
                ProjectID = 123,
                Hours = 8,
                Date = DateTime.Today},

                new()
                {
                UserID = 345,
                ProjectID = 345,
                Hours = 10,
                Date = DateTime.Today},

                new()
                {
                UserID = 765,
                ProjectID = 765,
                Hours = 10,
                Date = DateTime.Today}

            };

            foreach (TimesheetEntry entry in entries)
            {
                _service.AddEntry(entry);
            }

            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Equal(3, allEntries.Count);
        }
    }
}