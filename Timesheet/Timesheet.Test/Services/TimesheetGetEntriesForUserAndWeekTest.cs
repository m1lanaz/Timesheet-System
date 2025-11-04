using Timesheet.Components.Models;
using Timesheet.Components.Services;
using Xunit;

namespace Timesheet.Test.Services
{
    public class TimesheetGetEntriesForUserAndWeekTest
    {
        private readonly TimesheetService _service;

        public TimesheetGetEntriesForUserAndWeekTest()
        {
            _service = new TimesheetService();
        }

        //Passes if retrieves all entries for a given user in a given week
        [Fact]
        public void ReturnsEntries()
        {
            var userId = 1;
            var weekStart = new DateTime(2025, 11, 3);

            _service.AddEntry(new TimesheetEntry
            {
                UserID = userId,
                ProjectID = 101,
                Hours = 5.5m,
                Date = new DateTime(2025, 11, 3)
            });

            _service.AddEntry(new TimesheetEntry
            {
                UserID = userId,
                ProjectID = 102,
                Hours = 3.0m,
                Date = new DateTime(2025, 11, 4)
            });

            var result = _service.GetEntriesForUserAndWeek(userId, weekStart);

            Assert.True(result.Success);
            Assert.NotNull(result.Entries);
            Assert.Equal(2, result.Entries.Count);
            Assert.Contains(result.Entries, e => e.ProjectID == 101);
            Assert.Contains(result.Entries, e => e.ProjectID == 102);
        }

        //Passes if detects user does not exist
        [Fact]
        public void UserNotFound()
        {
            var weekStart = new DateTime(2025, 11, 3);

            var result = _service.GetEntriesForUserAndWeek(999, weekStart);

            Assert.False(result.Success);
            Assert.Equal("UserID 999 not found.", result.Message);
            Assert.Empty(result.Entries);
        }

        //Passes if sees user exists but has no entries in that week
        [Fact]
        public void NoEntriesInWeek()
        {
            var userId = 1;
            _service.AddEntry(new TimesheetEntry
            {
                UserID = userId,
                ProjectID = 200,
                Hours = 8,
                Date = new DateTime(2025, 10, 15) 
            });

            var weekStart = new DateTime(2025, 11, 3);
            var result = _service.GetEntriesForUserAndWeek(userId, weekStart);

            Assert.False(result.Success);
            Assert.Contains("No entries found", result.Message);
            Assert.Empty(result.Entries);
        }

        //Passes if correctly filters multiple users
        [Fact]
        public void CorrectUserFiltered()
        {
            var weekStart = new DateTime(2025, 11, 3);

            _service.AddEntry(new TimesheetEntry
            {
                UserID = 1,
                ProjectID = 101,
                Hours = 4,
                Date = new DateTime(2025, 11, 3)
            });

            _service.AddEntry(new TimesheetEntry
            {
                UserID = 2,
                ProjectID = 999,
                Hours = 9,
                Date = new DateTime(2025, 11, 3)
            });

            var result = _service.GetEntriesForUserAndWeek(1, weekStart);

            Assert.True(result.Success);
            Assert.Single(result.Entries);
            Assert.All(result.Entries, e => Assert.Equal(1, e.UserID));
        }
    }
}
