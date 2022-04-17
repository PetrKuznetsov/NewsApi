namespace NewsApi.Models
{
    public class UserConstants
    {
        public static List<UserViewModel> Users = new List<UserViewModel>()
        {
            new UserViewModel() { Username = "petr", EmailAddress = "darksider92@mail.ru", Password = "Qwerty123", GivenName = "Petr", Surname = "Kuznetsov", Role = "Administrator" },
        };
    }
}
