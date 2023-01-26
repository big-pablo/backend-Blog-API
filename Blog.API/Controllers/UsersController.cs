using Blog.API.Exceptions;
using Blog.API.Models.DTOs;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IInnerService _innerService;
        public UsersController(IAuthService authInterface, IUserService userService, IInnerService innerService)
        {
            _authService = authInterface;
            _userService = userService;
            _innerService = innerService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<TokenResponseDTO>> Register(UserRegisterDTO model)
        {
            try
            {
                return await _authService.Register(model);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        [HttpPost("login")]

        public async Task<ActionResult<TokenResponseDTO>> Login(LoginCredentialsDTO model)
        {
            try
            {
                return await _authService.Login(model);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            var token = await _innerService.GetToken(HttpContext.Request.Headers);
            try 
            {
                await _authService.Logout(token);
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserData()
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                var response = await _userService.GetUserProfile(await _innerService.GetUserId(HttpContext.User));
                return Ok(response);
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(UserEditDTO model)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                await _userService.UpdateUserProfile(await _innerService.GetUserId(HttpContext.User), model);
                return Ok();
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception) 
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
