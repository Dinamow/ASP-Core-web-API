using loginService.Models;

namespace loginService.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(User user);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user, Dictionary<string, string>? filter = null);
        bool DeleteUser(User user);
        Task<int> Save();
        List<User> GetUsers();
        Task<User> GetUserById(Guid id);
        Task<User> GetUserBy(Dictionary<string, string> filter);
        Task<string> Login(string email, string password);
        Task<bool> Logout(string sessionId);
        Task SignUp(Dictionary<string, string> dic);
    }
}
