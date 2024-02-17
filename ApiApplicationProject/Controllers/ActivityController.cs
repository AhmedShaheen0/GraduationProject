using ApiApplicationProject.Models;
using ApiApplicationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplicationProject.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]

    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IActivityService _actionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivityController(IActivityService actionService, UserManager<ApplicationUser> userManager, ILogger<ActivityController> logger)
        {
            _actionService = actionService;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet("Event")]
        public async Task<IActionResult> GetEvents()
        {
            var actions = _actionService.GetEvents();
            return Ok(actions);
        }
        [HttpGet("Event/Id")]
        public async Task<IActionResult> GetEventAsync(int id)
        {
            var action = _actionService.GetEventById(id);
            if (action is null) return NotFound();

            return Ok(action);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> PostEvent(ActivityModel activity )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByIdAsync(activity.ApplicationUserId);

            _actionService.CreateEvent(activity, user.Id);


            return Ok();
        }

        [HttpGet("Log")]
        public async Task<IActionResult> Getlog()
        {
            var actions = _actionService.GetLog();
            return Ok(actions);
        }
    }
}

