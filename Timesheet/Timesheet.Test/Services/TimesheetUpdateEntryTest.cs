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
            var entry = new TimesheetEntry
            {
                UserID = 123,
                ProjectID = 123,
                Hours = 8,
                Date = DateTime.Today
            };

            AddEntryResult added = _service.AddEntry(entry);

            var updatedValues = new TimesheetEntry
            {
                ProjectID = 867,
                Hours = 10
            };

            UpdateEntryResult result = _service.UpdateEntry(added.Entry.ID.ToString(), updatedValues);

            Assert.True(result.Success);
            Assert.NotNull(result.Entry);
            Assert.Equal(867, result.Entry.ProjectID);
            Assert.Equal(10, result.Entry.Hours);
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

            UpdateEntryResult result = _service.UpdateEntry("12234", updatedValues);

            Assert.False(result.Success);
            Assert.Equal("Entry not found.", result.Message);
            Assert.Null(result.Entry);
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

            foreach (var e in entries)
                _service.AddEntry(e);

            var targetId = _service.GetAllEntries()[0].ID.ToString();

            var updatedValues = new TimesheetEntry
            {
                ProjectID = 867,
                Hours = 10
            };

            UpdateEntryResult result = _service.UpdateEntry(targetId, updatedValues);

            Assert.True(result.Success);
            Assert.NotNull(result.Entry);
            Assert.Equal(867, result.Entry.ProjectID);
            Assert.Equal(10, result.Entry.Hours);

            //Checks only one entry was updated
            var all = _service.GetAllEntries();
            Assert.Single(all.Where(e => e.ProjectID == 867));

        }


        //Passes if detects duplicate and prevents it
        [Fact]
        public void UpdateCausingDuplicateFails()
        {
            var entry1 = new TimesheetEntry
            {
                UserID = 1,
                ProjectID = 10,
                Date = DateTime.Today,
                Hours = 8
            };

            var entry2 = new TimesheetEntry
            {
                UserID = 1,
                ProjectID = 20,
                Date = DateTime.Today,
                Hours = 6
            };

            _service.AddEntry(entry1);
            _service.AddEntry(entry2);

            var updatedValues = new TimesheetEntry
            {
                UserID = 1,
                ProjectID = 10,
                Date = DateTime.Today
            };

            var result = _service.UpdateEntry(entry2.ID.ToString(), updatedValues);

            Assert.False(result.Success);
            Assert.Equal("Duplicate entry exists for this user, project, and date.", result.Message);
        }
    }
}