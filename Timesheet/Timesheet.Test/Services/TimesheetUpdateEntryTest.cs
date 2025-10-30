using Timesheet.Components.Models;
using Timesheet.Components.Services;
using Xunit;

namespace Timesheet.Test.Services
{
    public class TimesheetUpdateEntryTest
    {

        private TimesheetService _service;

        public TimesheetUpdateEntryTest()
        {
            _service = new TimesheetService();
        }

        //Passes if updates an existing entry
        [Fact]
        public void UpdateExistingEntry()
        {
            TimesheetEntry entry = new TimesheetEntry
            {
                UserID = 123,
                ProjectID = 123,
                Hours = 8,
                Date = DateTime.Today
            };

            TimesheetEntry added = _service.AddEntry(entry);

            var updatedValues = new TimesheetEntry
            {
                ProjectID = 867,   
                Hours = 10      
            };


            TimesheetEntry updated = _service.UpdateEntry(added.ID, updatedValues);

            Assert.Equal(867, updated.ProjectID);
            Assert.Equal(10, updated.Hours);
        }

        //Update a non-existent entry
        [Fact]
        public void UpdateNonExistentEntry()
        {
            var updatedValues = new TimesheetEntry
            {
                ProjectID = 867,
                Hours = 10
            };

            TimesheetEntry updated = _service.UpdateEntry(Guid.NewGuid(), updatedValues);

            Assert.Null(updated);
        }

        //Passes if updates the correct entry
        [Fact]
        public void UpdateExistingEntryMultiple()
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

            var updatedValues = new TimesheetEntry
            {
                ProjectID = 867,
                Hours = 10
            };


            TimesheetEntry updated = _service.UpdateEntry(entries[0].ID, updatedValues);

            Assert.Equal(867, entries[0].ProjectID);
            Assert.Equal(10, entries[0].Hours);

        }
    }
}