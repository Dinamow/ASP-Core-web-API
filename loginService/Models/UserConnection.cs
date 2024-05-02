namespace loginService.Models
{
    public class UserConnection
    {
        public Guid User1Id { get; set; }
        public required User User1 { get; set; }

        public Guid User2Id { get; set; }
        public required User User2 { get; set; }
    }
}
