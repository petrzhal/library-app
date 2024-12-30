using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Library.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController(
        IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
            => Ok(await _mediator.Send(request));

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
            => Ok(await _mediator.Send(request));

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _mediator.Send(new LogoutRequest());
            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(await _mediator.Send(new GetCurrentUserRequest()));
        }
    }
}
