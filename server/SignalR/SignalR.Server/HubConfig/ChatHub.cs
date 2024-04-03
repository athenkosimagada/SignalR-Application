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
        private bool isLogout;
        public ChatHub(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            isLogout = false;
        }
        public async Task OnLogin(string userId)
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
            await Clients.Caller.SendAsync("OnLoggedIn", currentSignalrID);
        }

        public async Task OnLogout(string userId)
        {
            var result = _db.Connections.FirstOrDefault(u => u.UserId == userId);
            if (result != null) 
            { 
                result.SignalrId = "OFFLINE";
                _db.Connections.Update(result);
                await _db.SaveChangesAsync();
            }

            await Clients.All.SendAsync("Logout", userId);
        }

        public async Task SendMessage(object message, string recieverId)
        {
            
            var connectionReceiver = _db.Connections.FirstOrDefault(u => u.UserId == recieverId);
            if (!string.IsNullOrEmpty(connectionReceiver?.SignalrId))
            {
                await Clients.Client(connectionReceiver.SignalrId).SendAsync("RecieveMessage", message);
            }
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var result = _db.Connections.FirstOrDefault(u => u.SignalrId == Context.ConnectionId);

            if(result is not null)
            {
                _db.Connections.Remove(result);
                _db.SaveChangesAsync();
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
