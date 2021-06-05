using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Frank.Scheduler.Api.Controllers
{
    [ApiController]
    [Route("scheduler/tasks")]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            return Ok();
        }
    }
}
