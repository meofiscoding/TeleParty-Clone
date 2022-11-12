using Microsoft.AspNetCore.SignalR;
using Teleparty.Data;
using Teleparty.Models;

namespace Teleparty
{
    public class Chathub : Hub
    {
        public readonly static List<ApplicationUser> _Users = new List<ApplicationUser>();
        private ApplicationDbContext _applicationDbContext;
        public readonly static List<ApplicationUser> _Connections = new List<ApplicationUser>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        public Chathub(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        //Add all user & connection id into connectionMap
        public override async Task OnConnectedAsync()
        {
            if (_Users.Count == 0)
            {
                foreach (ApplicationUser users in _applicationDbContext.Users.ToList())
                {
                    _Users.Add(users);
                }
            }
            var user = _applicationDbContext.Users.Where(u => u.UserName == Context.User.Identity.Name).FirstOrDefault();
            user.ConnectionId = Context.ConnectionId;
            _Connections.Add(user);
            _ConnectionsMap.Add(Context.User.Identity.Name, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public Task SendMessageToGroup(string group, string message)
        {
            return Clients.Group(group).SendAsync("ReceiveMessage", message);
        } 
    }
}
