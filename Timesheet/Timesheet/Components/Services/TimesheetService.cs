using Timesheet.Components.Models;
using System.Collections.Concurrent;

namespace Timesheet.Components.Services
{
    public class TimesheetService
    {
        private readonly ConcurrentDictionary<string, TimesheetEntry> _entries = new();

        public TimesheetService() {

            var initialEntry = new TimesheetEntry
            {
                ID = Guid.NewGuid(),
                UserID = 1,
                ProjectID = 101,
                Hours = 7,
                Date = DateTime.Today,
                Description = "Initial test entry"
            };

            _entries.TryAdd(initialEntry.ID.ToString(), initialEntry);

        }

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
                return new AddEntryResult
                {
                    Success = false,
                    Message = "Failed adding entry"
                };
            }


        }

        public bool DeleteEntry(string id)
        {
            return _entries.TryRemove(id, out _);
        }


        public List<TimesheetEntry> GetAllEntries() 
        { 
            return _entries.Values.ToList();
        }


        public TimesheetEntry? UpdateEntry(string id, TimesheetEntry updatedValues)
        {
            if (_entries.TryGetValue(id, out var existingEntry))
            {
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

                return existingEntry;
            }

            return null;
        }

    }
}
