using Timesheet.Components.Models;
using Timesheet.Components.Services;
using Xunit;

namespace Timesheet.Test.Services
{
    public class TimesheetWeeklyHoursTests
    {
        private TimesheetService CreateServiceWithSampleData()
        {
            var service = new TimesheetService();

            //sample entries for user
            service.AddEntry(new TimesheetEntry
            {
                UserID = 1,
                ProjectID = 101,
                Hours = 5.5m,
                Date = new DateTime(2025, 11, 3)
            });

            service.AddEntry(new TimesheetEntry
            {
                UserID = 1,
                ProjectID = 102,
                Hours = 3.0m,
                Date = new DateTime(2025, 11, 4)
            });

            return service;
        }

        //Get all entries with a fake user
        [Fact]
        public void GetWeeklyProjectHoursForFakeUser()
        {
            var service = CreateServiceWithSampleData();

            var result = service.GetWeeklyProjectHoursByUser(userId: 999, weekStart: new DateTime(2025, 11, 3));

            Assert.False(result.Success);
            Assert.Equal("UserID 999 not found.", result.Message);
            Assert.Empty(result.Results);
        }

        //Existing user but wrong week
        [Fact]
        public void GetWeeklyProjectHoursForWrongWeek()
        {
            var service = CreateServiceWithSampleData();

            //Week with no data
            var result = service.GetWeeklyProjectHoursByUser(userId: 1, weekStart: new DateTime(2025, 1, 1));

            Assert.False(result.Success);
            Assert.Contains("No projects assigned", result.Message);
            Assert.Empty(result.Results);
        }

        [Fact]
        public void GetWeeklyProjectHoursForRealUser()
        {
            var service = CreateServiceWithSampleData();

            var result = service.GetWeeklyProjectHoursByUser(userId: 1, weekStart: new DateTime(2025, 11, 3));

            Assert.True(result.Success);
            Assert.NotEmpty(result.Results);

            //Should be 2 project groups
            Assert.Equal(2, result.Results.Count);

            var project101 = result.Results.FirstOrDefault(r => r.ProjectID == 101);
            var project102 = result.Results.FirstOrDefault(r => r.ProjectID == 102);

            Assert.NotNull(project101);
            Assert.NotNull(project102);

            Assert.Equal(5.5m, project101.TotalHours);
            Assert.Equal(3.0m, project102.TotalHours);
        }
    } 
}