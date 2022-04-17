using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsApi.Interfaces;
using NewsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;

        public LoginController(IConfiguration config,
               ILogger<LoginController> logger,
               ILoginService loginService)
        {
            _config = config;
            _logger = logger;
            _loginService = loginService;
        }

        /// <summary>
        /// Аутентификация пользователя в API 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Login([FromBody] UserLoginViewModel userLogin)
        {
            try
            {
                _logger.LogInformation($"Пользователь {userLogin.Username} пытается пройти аутентификацию");
                var user = _loginService.Authenticate(userLogin);
                if (user != null)
                {
                    var token = _loginService.GenerateToken(user);
                    _logger.LogInformation("Токен успешно получен");
                    return Ok(token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка" + ex);
                return StatusCode(500);
            }
            return NotFound("Пользователь не найден");
        }
    }
}
