using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Timesheet.Components.Models;
using Timesheet.Components.Services;

namespace Timesheet.Components.Controllers
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
                return BadRequest(result.Message);
            }


            return Ok(result.Entry);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEntry(string id) 
        {
            bool success = _service.DeleteEntry(id);

            if (success)
            {
                return Ok("Entry deleted");
            }
            else
            {
                return BadRequest("Issue with deleting entry");
            }
        }
        [HttpPut("{id}")]
        public ActionResult UpdateEntry(string id, TimesheetEntry updatedData)
        {
            var result = _service.UpdateEntry(id, updatedData);

            if (!result.Success)
            {
                if (result.Message == "Entry not found.")
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Entry);
        }


    }
}
