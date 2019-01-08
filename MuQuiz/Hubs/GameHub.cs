using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Hubs
{
    public class GameHub : Hub
    {
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendSong(string song)
        {
            await Clients.Others.SendAsync("newsong", song);
        }
    }
}
