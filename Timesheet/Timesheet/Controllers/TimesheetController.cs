using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Timesheet.Components.Models;
using Timesheet.Components.Services;

namespace Timesheet.Controllers
{
    [Route("api/timesheet")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly TimesheetService _service;

        public TimesheetController(TimesheetService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<TimesheetEntry>> GetAllEntries()
        {
            return _service.GetAllEntries();
        }

        [HttpPost]
        public ActionResult AddEntry(TimesheetEntry entry)
        {
            var result = _service.AddEntry(entry);
            if (!result.Success)
            {
                return Conflict(result.Message);
            }

            return Ok(result.Entry);
        }

        [HttpDelete]
        public ActionResult DeleteEntry(TimesheetEntry entry) 
        {
            bool success = _service.DeleteEntry(entry.ID.ToString());

            if (success)
            {
                return Ok("Entry deleted");
            }
            else
            {
                return Conflict("Issue with deleting entry");
            }
        }

        [HttpPut]
        public ActionResult UpdateEntry(TimesheetEntry entryToUpdate, TimesheetEntry updatedData)
        {
            TimesheetEntry updatedTimesheetEntry = _service.UpdateEntry(entryToUpdate.ID, updatedData);
        
            if( updatedTimesheetEntry == null)
            {
                return Conflict("Issue with updating entry");
            }

            return Ok(updatedTimesheetEntry);
        }

    }
}
