using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loginService.Models
{
    public class Connection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // The user who sent the connection request
        public Guid SenderUserId { get; set; }
        [ForeignKey("SenderUserId")]
        public required User SenderUser { get; set; }

        // The user who received the connection request
        public Guid ReceiverUserId { get; set; }
        [ForeignKey("ReceiverUserId")]
        public required User ReceiverUser { get; set; }

        public bool IsAccepted { get; set; } = false;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}
