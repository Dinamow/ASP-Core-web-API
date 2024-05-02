using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loginService.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        private string? Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public required string Phone { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public required string Gender { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set; }
        public bool IsActive { get; set; } = false;
        public string? SessionId { get; set; }
        public string? ResetToken { get; set; }
        public string? ResetTokenExpire { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Connection>? SentConnections { get; set; }
        public ICollection<Connection>? ReceivedConnections { get; set; }
        public void setPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password is required");
            }
            if (password.Length < 8)
            {
                throw new Exception("Password must be at least 8 characters long");
            }
            if (!password.Any(char.IsDigit))
            {
                throw new Exception("Password must contain at least one digit");
            }
            if (!password.Any(char.IsUpper))
            {
                throw new Exception("Password must contain at least one uppercase letter");
            }
            if (!password.Any(char.IsLower))
            {
                throw new Exception("Password must contain at least one lowercase letter");
            }
            this.Password = HashPassword(password);
        }
        private string HashPassword(string password)
        {
            // Generate a salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }
        public bool VerifyPassword(string password)
        {
            // Verify the password against the hashed password
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, this.Password);

            return passwordMatches;
        }
    }
}
