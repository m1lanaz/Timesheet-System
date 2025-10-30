using Timesheet.Components.Models;
using System.Collections.Concurrent;

namespace Timesheet.Components.Services
{
    public class TimesheetService
    {
        private readonly ConcurrentDictionary<string, TimesheetEntry> _entries = new();


        public TimesheetEntry AddEntry(TimesheetEntry entry)
        {
            entry.ID = Guid.NewGuid();
            _entries.TryAdd(entry.ID.ToString(), entry);
            return entry;
        }

        public bool DeleteEntry(string id)
        {
            return _entries.TryRemove(id, out _);
        }


        public List<TimesheetEntry> GetAllEntries() 
        { 
            return _entries.Values.ToList();
        }


        public TimesheetEntry? UpdateEntry(Guid id, TimesheetEntry updatedValues)
        {
            if (_entries.TryGetValue(id.ToString(), out var existingEntry))
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
