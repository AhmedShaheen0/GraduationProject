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
    public class ActionsController : ControllerBase
    {
        private readonly IActionService _actionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActionsController(IActionService actionService, UserManager<ApplicationUser> userManager)
        {
            _actionService = actionService;
            _userManager = userManager;
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
            if (action is null)   return NotFound();
          
            return Ok(action);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> PostEvent(EvntModel action)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByIdAsync(action.ApplicationUserId);
      
            _actionService.CreateEvent(action, user.Id);


            return Ok();
        }
    }
}
