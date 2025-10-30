using Timesheet.Components.Models;
using Timesheet.Components.Services;
using Xunit;

namespace Timesheet.Test.Services
{
    public class TimesheetAddDeleteTests
    {

        private TimesheetService _service;

        public TimesheetAddDeleteTests()
        {
            _service = new TimesheetService();
        }

        //Passes if it deletes entry
        [Fact]
        public void DeleteEntry()
        {
            TimesheetEntry entry = new TimesheetEntry
            {
                UserID = 123,
                ProjectID = 123,
                Hours = 8,
                Date = DateTime.Today
            };

            TimesheetEntry added = _service.AddEntry(entry);

            bool deleted = _service.DeleteEntry(added.ID.ToString());

            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Empty(allEntries);
        }


        //Passes if a fake entry that isn't there
        [Fact]
        public void DeleteFakeEntry()
        {
            bool deleted = _service.DeleteEntry("12345test");

            Assert.False(deleted);
        }

        //Passes if only deletes one entry
        [Fact]
        public void DeleteOneEntryFromMultiple()
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

            bool deleted = _service.DeleteEntry(entries[1].ID.ToString());

            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Equal(2, allEntries.Count);

        }
    }
}