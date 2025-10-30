using Timesheet.Components.Models;
using Timesheet.Components.Services;
using Xunit;

namespace Timesheet.Test.Services
{
    public class TimesheetAddEntryTests
    {

        private TimesheetService _service;

        public TimesheetAddEntryTests()
        {
            _service = new TimesheetService(); 
        }

        //Passes if it adds entry
        [Fact]
        public void AddOneEntry()
        {
            TimesheetEntry entry = new TimesheetEntry
            {
                UserID = 123,
                ProjectID = 123,
                Hours = 8,
                Date = DateTime.Today
            };

            AddEntryResult added = _service.AddEntry(entry);
            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Contains(allEntries, e => e.ID == added.Entry.ID);
        }

        //Passes if it adds all entries
        [Fact]
        public void AddMultipleEntries()
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

            foreach(TimesheetEntry entry in entries)
            {
                _service.AddEntry(entry);
            }

            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Equal(3, allEntries.Count);

        }

        //Passes if it doesn't allow to add duplicate Ids
        [Fact]
        public void AddDuplicateEntries()
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
                UserID = 123,
                ProjectID = 123,
                Hours = 10,
                Date = DateTime.Today},

            };

            foreach (TimesheetEntry entry in entries)
            {
                _service.AddEntry(entry);
            }

            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.Single(allEntries);
        }

        //Passes if it handles null values
        [Fact]
        public void AddEmptyEntry()
        {
            TimesheetEntry entry = new TimesheetEntry();

            AddEntryResult added = _service.AddEntry(entry);
            List<TimesheetEntry> allEntries = _service.GetAllEntries();

            Assert.NotNull(added.Entry);
            Assert.Contains(added.Entry, allEntries);
        }
    }
}