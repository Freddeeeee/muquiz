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
            await service.AddPlayer(Context.ConnectionId, name, gameId);
            var host = await service.GetHostConnectionIdByGameId(gameId);
            await Clients.Client(host).ReceiveConnectedPlayerName(name);
        }

        public async Task SendSong(string group, string song)
        {
            await Clients.Group(group).ReceiveSong(song);
        }

        public async Task SendAnswer(string answer, string gameId)
        {
            var playerInfo = await service.GetPlayerByConnectionId(Context.ConnectionId);
            var name = playerInfo.Name;
            var hostConnectionId = await service.GetHostConnectionIdByGameId(gameId);

            await Clients.Client(hostConnectionId).ReceiveAnswer(answer, name);
            service.EvaluateAnswer(Context.ConnectionId, answer);
        }

        public async Task SendToWaitingScreen(string group)
        {
            await Clients.Group(group).GetWaitingScreen();
        }

        public async Task SendToFinalPosition(string group)
        {
            var players = await service.GetAllPlayers(group);

            for (int i = 0; i < players.Length; i++)
            {
                await Clients.Client(players[i].ConnectionId).GetFinalPosition(i + 1);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (await service.IsPlayer(Context.ConnectionId))
            {
                await service.RemovePlayerByConnectionId(Context.ConnectionId);
            }
            else
            {
                var gameId = await service.GetGameIdByConnectionId(Context.ConnectionId);
                await Clients.Group(gameId).GetSessionClosedScreen();
                await service.RemoveGameSession(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
