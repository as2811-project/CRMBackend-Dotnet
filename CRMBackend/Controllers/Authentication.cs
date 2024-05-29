using CRMBackend.Contracts;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace CRMBackend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly Client _client;

        public AuthController(Client client)
        {
            _client = client;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthService.Login request)
        {
            try
            {
                var session = await _client.Auth.SignIn(request.email, request.password);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(AuthService.Login request)
        {
            try
            {
                var attrs = new UserAttributes { Password = request.password };
                var response = await _client.Auth.Update(attrs);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthService.CreateUser request)
        {
            try
            {
                var response = await _client.Auth.SignUp(request.email, request.password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }
    }
}