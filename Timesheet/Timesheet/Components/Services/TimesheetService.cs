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


        public TimesheetEntry? UpdateEntry(Guid id, TimesheetEntry entryToUpdate)
        {
            if(_entries.TryGetValue(id.ToString(), out var existingEntry))
            {
                entryToUpdate.ID = id;
                _entries[id.ToString()] = entryToUpdate;
                return existingEntry;
            }
            return null;
        }
    }
}
