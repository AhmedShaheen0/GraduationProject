using ApiApplicationProject.Models;
using ApiApplicationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Net.NetworkInformation;

namespace ApiApplicationProject.Controllers
{
     [Route("api/[controller]")]
  
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServiece _authserviece;

        public AuthController(IAuthServiece authserviece)
        {
            _authserviece = authserviece;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegesterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.Age < 10 || model.Age > 100)
                return BadRequest(" Enter your real age !! ");
            if (model.Age < 10 || model.Age > 100)
                return BadRequest(" Enter your real age !! ");

            var result = await _authserviece.RegestrAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authserviece.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authserviece.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }
}