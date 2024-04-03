using System.ComponentModel.DataAnnotations;

namespace Auth.Server.Models
{
    public class UserMessage
    {
        [Key]
        public Guid MessageId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
    }
}
