using loginService.Data;
using loginService.Interfaces;
using loginService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace loginService.Repository
{
    public class UserRepository(DataContext Context) : IUserRepository
    {
        
        private readonly string[] req = ["Email", "Gender", "Username", "Phone", "Password"];
        private readonly DataContext _context = Context;
        public async Task<string> Login(string email, string password)
        {
            try
            {
                var user = await this.GetUserBy(new Dictionary<string, string> { { "Email", email } });
                if (user.VerifyPassword(password))
                {
                    user.SessionId = Guid.NewGuid().ToString();
                    UpdateUser(user).Wait();
                    Save().Wait();
                    return user.SessionId;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Invalid credentials", e);
            }
            throw new Exception("Invalid credentials");
        }

        public async Task<bool> Logout(string sessionId)
        {
            try
            {
                var user = await this.GetUserBy(new Dictionary<string, string> { { "SessionId", sessionId } });
                user.SessionId = null;
                UpdateUser(user).Wait();
                Save().Wait();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Something went Wrong", e);
            }
        }
        public async Task SignUp(Dictionary<string, string> dic)
        {
            foreach (var item in req)
                if (!dic.TryGetValue(item, out string? value))
                    throw new MissingFieldException($"{item} is required");
            if (!IsValidEmail(dic["Email"]))
                throw new Exception("Invalid email address");
            var user = new User()
            {
                Email = dic["Email"],
                Phone = dic["Phone"],
                Gender = dic["Gender"],
                Username = dic["Username"],
                Role = dic["Email"] == "meemoo102039@gmail.com" ? "Admin" : "user"
            };
            user.SetPassword(dic["Password"]);
            await CreateUser(user);
        }

        private static bool IsValidEmail(string v)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            Regex emailRegex = new Regex(pattern);

            // Use the regex to validate the email
            return emailRegex.IsMatch(v);
        }

        public static bool IsHashedPassword(string password)
        {
            // Regular expression pattern to match bcrypt hashed passwords
            string bcryptPattern = @"^\$2[ayb]\$.{56}$";

            // Check if the password matches the bcrypt pattern
            return Regex.IsMatch(password, bcryptPattern);
        }
        public Task<int> Save()
        {
            return this._context.SaveChangesAsync();
        }
        public bool UserExists(User user)
        {
            if (user == null)
                return false;
            if (this._context.Users.Any(u => u.Email == user.Email || u.Username == user.Username))
                return true;
            return false;
        }
        public List<User> GetUsers()
        {
            return this._context.Users.ToList();
        }
        public async Task<User> GetUserById(Guid id)
        {
            return await this._context.Users.FindAsync(id) ?? throw new Exception("No User found");
        }
        public async Task<User> GetUserBy(Dictionary<string, string> filter)
        {
            if (filter == null || filter.Count == 0)
                throw new ArgumentException("No filter was provided", nameof(filter));

            IQueryable<User> query = this._context.Users.AsQueryable();

            foreach (var item in filter)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                    throw new ArgumentException("Invalid filter key or value");

                var property = typeof(User).GetProperty(item.Key);

                if (property == null)
                    throw new ArgumentException($"Property '{item.Key}' does not exist in User class");

                ParameterExpression parameter = Expression.Parameter(typeof(User), "u");
                MemberExpression member = Expression.Property(parameter, property);
                ConstantExpression constant = Expression.Constant(item.Value, typeof(string));
                BinaryExpression equal = Expression.Equal(member, constant);
                Expression<Func<User, bool>> lambda = Expression.Lambda<Func<User, bool>>(equal, parameter);

                query = query.Where(lambda);
            }
            return await query.FirstOrDefaultAsync() ?? throw new Exception("No User Found");
        }
        public async Task<User> CreateUser(User user)
        {
            if (user.GetType() == null)
                throw new Exception("User is required");
            if (UserExists(user))
                throw new Exception("User Already Exists");
            foreach (var item in req)
            {
                if (user.GetType().GetProperty(item) == null)
                    throw new Exception($"{item} is required");
                if (string.IsNullOrEmpty(user.GetType().GetProperty(item).GetValue(user).ToString()))
                    throw new Exception($"{item} is required");
            }
            if (!IsHashedPassword(user.Password)) throw new Exception("Please Use User.SetPassword()");
            await this._context.Users.AddAsync(user);
            Save().Wait();
            return user;
        }
        public async Task<User> UpdateUser(User user, Dictionary<string, string>? filter = null)
        {
            // i want to update user based on the dict values

            user.UpdatedAt = DateTime.UtcNow;
            await Save();
            return user;
        }
        public bool DeleteUser(User user)
        {
            try
            {
                this._context.Users.Remove(user);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("User not found", e);
            }
        }
    }
}
