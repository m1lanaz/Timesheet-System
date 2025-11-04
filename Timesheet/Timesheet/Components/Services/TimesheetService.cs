using Timesheet.Components.Models;
using System.Collections.Concurrent;

namespace Timesheet.Components.Services
{
    public class TimesheetService
    {
        private readonly ConcurrentDictionary<string, TimesheetEntry> _entries = new();


        //Add entry to timesheet entries
        public AddEntryResult AddEntry(TimesheetEntry entry)
        {
            //Duplicate check
           bool anyDuplicates = _entries.Values.Any(e =>
            e.UserID == entry.UserID &&
            e.ProjectID == entry.ProjectID &&
            e.Date == entry.Date);

            if (anyDuplicates)
            {
                return new AddEntryResult
                {
                    Success = false,
                    Message = "Duplicate entry found for this user, project, and date."
                };
            }

            entry.ID = Guid.NewGuid();
            bool added = _entries.TryAdd(entry.ID.ToString(), entry);

            //return entry if successful add 
            if (added)
            {
                return new AddEntryResult
                {
                    Success = true,
                    Entry = entry,
                };
            }
            else
            {
                //return message if unsuccessful add
                return new AddEntryResult
                {
                    Success = false,
                    Message = "Failed adding entry"
                };
            }


        }

        //Delete entry from timesheet entries
        public bool DeleteEntry(string id)
        {
            return _entries.TryRemove(id, out _);
        }

        //Get all entries from timesheet entries
        public List<TimesheetEntry> GetAllEntries() 
        { 
            return _entries.Values.ToList();
        }

        //Update entry from timesheet entries
        public UpdateEntryResult UpdateEntry(string id, TimesheetEntry updatedValues)
        {
            if (!_entries.TryGetValue(id, out var existingEntry))
            {
                return new UpdateEntryResult { Success = false, Message = "Entry not found." };
            }

                //Check if this would create a duplicate
                bool anyDuplicates = _entries.Values.Any(e =>
                    e.ID != existingEntry.ID && 
                    e.UserID == updatedValues.UserID &&
                    e.ProjectID == updatedValues.ProjectID &&
                    e.Date.Date == updatedValues.Date.Date);

            //If theres a potential duplicate found then return message
               if (anyDuplicates)
                {
                    return new UpdateEntryResult
                    {
                        Success = false,
                        Message = "Duplicate entry exists for this user, project, and date."
                    };
                }

            //Assigning new values
            if (updatedValues.ProjectID != default)
                {
                    existingEntry.ProjectID = updatedValues.ProjectID;
                }

                if (!string.IsNullOrWhiteSpace(updatedValues.Description))
                {
                    existingEntry.Description = updatedValues.Description;
                }

                if (updatedValues.Hours > 0)
                {
                    existingEntry.Hours = updatedValues.Hours;
                }

                if (updatedValues.Date != default)
                {
                    existingEntry.Date = updatedValues.Date;
                }

                _entries[id.ToString()] = existingEntry;

            //If successful then return entry
            return new UpdateEntryResult { Success = true, Entry = existingEntry };
        }


        //Return all projectIds with total hours for a given userId in a week
        public WeeklyProjectHoursResponse GetWeeklyProjectHoursByUser(int userId, DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(7);

            //First check that this userID exists
            var userEntries = _entries.Values
                .Where(e => e.UserID == userId)
                .ToList();

            if (!userEntries.Any())
            {
                return new WeeklyProjectHoursResponse
                {
                    Success = false,
                    Message = $"UserID {userId} not found."
                };
            }

            //Second check if the user exists but they have no projectids assigned for the selected week

            var weeklyEntries = userEntries
               .Where(e => e.Date >= weekStart && e.Date < weekEnd)
               .ToList();

            if (!weeklyEntries.Any())
            {
                return new WeeklyProjectHoursResponse
                {
                    Success = false,
                    Message = $"No projects assigned to user {userId} in this timeframe."
                };
            }

            //Then group by project and total hours

            var results = weeklyEntries
                .GroupBy(e => e.ProjectID)
                .Select(g => new ProjectHoursResult
                {
                    ProjectID = g.Key,
                    TotalHours = g.Sum(e => e.Hours)
                })
                .ToList();

            return new WeeklyProjectHoursResponse
            {
                Success = true,
                Results = results
            };

        }
    }
}
