namespace SignalR.Server.Models.Dto
{
    public class ConnectionDto
    {
        public Guid ConnectionId { get; set; }
        public string UserId { get; set; }
        public string SignalrId { get; set; }
    }
}
