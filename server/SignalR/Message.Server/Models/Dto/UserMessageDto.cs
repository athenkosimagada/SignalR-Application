namespace Message.Server.Models.Dto
{
    public class UserMessageDto
    {
        public Guid MessageId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
    }
}
