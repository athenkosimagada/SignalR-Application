namespace SignalR.Server.Models
{
    public class Connection
    {
        public Guid ConnectionId { get; set; }
        public string UserId { get; set; }
        public string SignalrId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
