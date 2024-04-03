namespace SignalR.Server.Models.Dto
{
    public class MessageDto
    {
        public Guid messageId { get; set; }
        public string fromUserId { get; set; }
        public string toUserId { get; set; }
        public string content { get; set; }
        public DateTime? sentOn { get; set; }
    }
}
