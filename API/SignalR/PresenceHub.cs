using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTraker _traker;
        public PresenceHub(PresenceTraker traker)
        {
            _traker = traker;
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await _traker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var currentUsers = await _traker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isOffline = await _traker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);

            if(isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());

            await base.OnDisconnectedAsync(exception);
        }
    }
}