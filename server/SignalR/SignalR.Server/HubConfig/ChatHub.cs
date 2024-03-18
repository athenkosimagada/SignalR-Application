using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Data;
using SignalR.Server.Models;
using SignalR.Server.Models.Dto;

namespace SignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public ChatHub(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task CreateConnection(string userId)
        {
            var result = _db.Connections.FirstOrDefault(u => u.UserId == userId);
            Connection connection;
            string currentSignalrID = Context.ConnectionId;
            if (result == null)
            {
                connection = new Connection
                {
                    UserId = userId,
                    SignalrId = currentSignalrID,
                    CreatedDate = DateTime.Now
                };

                await _db.Connections.AddAsync(connection);
                await _db.SaveChangesAsync();
            }
            else
            {
                result.SignalrId = currentSignalrID;
                connection = result;

                _db.Connections.Update(result);
                await _db.SaveChangesAsync();
            }
            await Clients.Caller.SendAsync("ConnectionCreated", connection);
        }

        public async Task SendMessage(string receiverId)
        {
            var connectionReceiver = _db.Connections.FirstOrDefault(u => u.UserId == receiverId);
            if (!string.IsNullOrEmpty(connectionReceiver?.SignalrId))
            {
                await Clients.Client(connectionReceiver.SignalrId).SendAsync("RecieveMessage", receiverId);
            }
        }
    }
}
