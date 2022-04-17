using NewsApi.Models;

namespace NewsApi.Interfaces
{
    public interface ILoginService
    {
        /// <summary>
        /// Создается токен для пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateToken(UserViewModel user);
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        UserViewModel Authenticate(UserLoginViewModel userLogin);
    }
}
