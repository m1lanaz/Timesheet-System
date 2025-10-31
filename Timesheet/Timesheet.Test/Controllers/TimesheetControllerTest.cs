using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Timesheet.Components.Models;
using Timesheet;

public class TimesheetControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TimesheetControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    //Passes if it returns all the list (list is empty)
    [Fact]
    public async Task GetAllEntries()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/timesheet");

        List<TimesheetEntry> entries = await response.Content.ReadFromJsonAsync<List<TimesheetEntry>>();
        Assert.NotNull(response);
    }


    //Passes if entry is added
    [Fact]
    public async Task AddEntry()
    {
        var entry = new TimesheetEntry
        {
            UserID = 12345,
            ProjectID = 16,
            Date = System.DateTime.UtcNow,
            Hours = 8,
            Description = "Test entry"
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/timesheet", entry);

        TimesheetEntry result = await response.Content.ReadFromJsonAsync<TimesheetEntry>();

        Assert.NotNull(result);

        Assert.Equal(entry.UserID, result.UserID);
    }

    //Passes if entry is deleted
    [Fact]
    public async Task DeleteEntry()
    {
        //Create entry
        var entry = new TimesheetEntry
        {
            UserID = 12345,
            ProjectID = 16,
            Date = System.DateTime.UtcNow,
            Hours = 8,
            Description = "Test entry"
        };

        HttpResponseMessage PostResponse = await _client.PostAsJsonAsync("/api/timesheet", entry);
        PostResponse.EnsureSuccessStatusCode();

        TimesheetEntry createdResult = await PostResponse.Content.ReadFromJsonAsync<TimesheetEntry>();

        //Delete 

        HttpResponseMessage deleteResponse = await _client.DeleteAsync($"/api/timesheet/{createdResult.ID}");

        //Check entry deleted

        HttpResponseMessage response = await _client.GetAsync("/api/timesheet");

        List<TimesheetEntry> entries = await response.Content.ReadFromJsonAsync<List<TimesheetEntry>>();

        Assert.Equal(0, entries.Count);
    }

    //Passes if entry is updated
    [Fact]
    public async Task UpdateEntry()
    {
        //Create entry
        var entry = new TimesheetEntry
        {
            UserID = 12345,
            ProjectID = 16,
            Date = System.DateTime.UtcNow,
            Hours = 8,
            Description = "Test entry"
        };

        HttpResponseMessage PostResponse = await _client.PostAsJsonAsync("/api/timesheet", entry);
        PostResponse.EnsureSuccessStatusCode();

        TimesheetEntry createdResult = await PostResponse.Content.ReadFromJsonAsync<TimesheetEntry>();

        //Update entry
        var updatedEntry = new TimesheetEntry
        {
            Hours = 20
        };

        HttpResponseMessage PutResponse = await _client.PutAsJsonAsync($"/api/timesheet/{createdResult.ID}", updatedEntry);
        PutResponse .EnsureSuccessStatusCode();

        TimesheetEntry UpdatedResult = await PutResponse.Content.ReadFromJsonAsync<TimesheetEntry>();

        Assert.Equal(20, UpdatedResult.Hours);
    }
}
