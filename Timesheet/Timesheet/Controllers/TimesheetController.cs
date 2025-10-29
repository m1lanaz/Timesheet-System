using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Timesheet.Controllers
{
    [Route("api/timesheet")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<TimesheetEntry>>> GetAllEntries()
        {

        }

    }
}
