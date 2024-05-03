using loginService.Models;

namespace loginService.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Connection>? SentConnections { get; set; }
        public ICollection<Connection>? ReceivedConnections { get; set; }
    }
}
