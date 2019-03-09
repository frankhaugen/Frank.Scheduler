using Microsoft.AspNetCore.Mvc;

namespace scheduled_job.Controllers
{
    public class HeartbeatController : ControllerBase
    {
		[HttpGet("/heartbeat")]
		public IActionResult Heartbeat()
		{
			return Ok("I'm ALIVE!");
		}
	}
}
