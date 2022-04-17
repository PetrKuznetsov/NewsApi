using Microsoft.IdentityModel.Tokens;
using NewsApi.Interfaces;
using NewsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsApi.Services
{
    public class LoginService : ILoginService
    {

        private readonly IConfiguration _config;
        private readonly ILogger<LoginService> _logger;
        public LoginService(IConfiguration config, ILogger<LoginService> logger)
        {
            _config=config;
            _logger=logger;
        }
        public UserViewModel Authenticate(UserLoginViewModel userLogin)
        {
            try
            {
                _logger.LogInformation($"Входные данные пользователя: {userLogin.Username}");
                var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

                if (currentUser != null)
                {
                    _logger.LogInformation($"Пользователь успешно получен: {userLogin.Username}");
                    return currentUser;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка" + ex);
                throw;
            }

            _logger.LogInformation("Пользователь не найден");
            return null;
        }

        public string GenerateToken(UserViewModel user)
        {
            try
            {
                _logger.LogInformation($"Для пользователя {user.Username} создается токен");
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddMinutes(15),
                  signingCredentials: credentials);
                _logger.LogInformation($"Пользователь {user.Username} успешно получил токен");
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка" + ex);
                return null;
            }
            
        }
    }
}
