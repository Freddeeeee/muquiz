using Microsoft.AspNetCore.SignalR;
using MuQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Hubs
{
    public class GameHub : Hub<IHubClient>
    {
        readonly GameService service;

        public GameHub(GameService service)
        {
            this.service = service;
        }

        public async Task StartSession(string gameId)
        {
            await service.InitializeSession(gameId, Context.ConnectionId);
        }

        public async Task StartGame(string gameId)
        {
            await service.StartPlaying(gameId);
        }

        public async Task AddToGroup(string gameId, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            service.AddPlayer(Context.ConnectionId, name, gameId);
            var host = service.GetHostConnectionId(gameId);
            await Clients.Client(host).ReceiveConnectedPlayerName(name);
        }

        public async Task SendSong(string group, string song)
        {
            await Clients.Group(group).ReceiveSong(song);
        }

        public async Task SendAnswer(string answer)
        {
            //to-do: change from All to sending to host only or group including host
            var playerInfo = service.GetPlayerByConnectionId(Context.ConnectionId);
            var name = playerInfo.Name;
            var hostConnectionId = service.GetGameSessionById(playerInfo.GameSessionId).HostConnectionId;

            //await Clients.All.ReceiveAnswer(answer, name);
            await Clients.Client(hostConnectionId).ReceiveAnswer(answer, name);
            service.EvaluateAnswer(Context.ConnectionId, answer);
        }

        public async Task SendToWaitingScreen(string group)
        {
            await Clients.Group(group).GetWaitingScreen();
        }

        public async Task SendToFinalPosition(string group)
        {
            var players = service.GetAllPlayers(group);

            for (int i = 0; i < players.Length; i++)
            {
                await Clients.Client(players[i].ConnectionId).GetFinalPosition(i + 1);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (service.IsPlayer(Context.ConnectionId))
            {
                service.RemovePlayerByConnectionId(Context.ConnectionId);
            }
            else
            {
                await Clients.Group(service.GetGroup(Context.ConnectionId)).GetSessionClosedScreen();
                service.RemoveAllPlayers(Context.ConnectionId);
                service.RemoveGameSession(Context.ConnectionId);
            }

            //return base.OnDisconnectedAsync(exception);
        }
    }
}
