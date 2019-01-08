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

        public async Task SendSong(string group, string song)
        {
            await Clients.Group(group).SendAsync("newsong", song);
            await Clients.Caller.SendAsync("newsong", song);
        }

        public async Task SubmitAnswer(string answer)
        {
            await Clients.All.SendAsync("receivedanswer");
        }

        public async Task GoToWaitingScreen(string group)
        {
            await Clients.Group(group).SendAsync("gotowaitingscreen");
        }

        public async Task SendFinalPosition(string group)
        {
            await Clients.Group(group).SendAsync("finalposition", 1);
        }
    }
}
