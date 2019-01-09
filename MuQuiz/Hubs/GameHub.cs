using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Hubs
{
    public class GameHub : Hub<IHubClient>
    {
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendSong(string group, string song)
        {
            await Clients.Group(group).ReceiveSong(song);
        }

        public async Task SendAnswer(string answer)
        {
            //to-do: change from All to sending to host only or group including host
            await Clients.All.ReceiveAnswer();
        }

        public async Task SendToWaitingScreen(string group)
        {
            await Clients.Group(group).GetWaitingScreen();
        }

        public async Task SendToFinalPosition(string group)
        {
            await Clients.Group(group).GetFinalPosition(1);
        }
    }
}
