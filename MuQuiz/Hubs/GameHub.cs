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
        private List<string> correctAnswerList = new List<string>();

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
            await SendNameToHost(gameId, name);
        }

        public async Task SendNameToHost(string gameId, string name)
        {
            var host = await service.GetHostConnectionIdByGameId(gameId);
            await Clients.Client(host).ReceiveConnectedPlayerName(name);
        }

        public async Task AskForJoinConfirmation(string gameId)
        {
            await Clients.Group(gameId).ConfirmJoined();
        }

        public async Task SendSong(string gameId, string song)
        {
            correctAnswerList.Clear();
            await Clients.Group(gameId).ReceiveSong(song);
        }

        public async Task SendAnswer(string answer, string gameId)
        {
            var playerInfo = await service.GetPlayerByConnectionId(Context.ConnectionId);
            var connectionId = Context.ConnectionId;
            var name = playerInfo.Name;
            var hostConnectionId = await service.GetHostConnectionIdByGameId(gameId);
            var correctAnswer = service.EvaluateAnswer(connectionId, answer);

            await Clients.Client(hostConnectionId).ReceiveAnswer(connectionId, correctAnswer, name);
        }

        public async Task UpdateScore(string connectionId, int position)
        {
            await service.UpdateScore(connectionId, position);
        }

        public async Task SendToWaitingScreen(string gameId)
        {
            await Clients.Group(gameId).GetWaitingScreen();
        }

        public async Task SendToFinalPosition(string gameId)
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
                var gameId = await service.GetGameIdByConnectionId(Context.ConnectionId);
                var hostConnectionId = await service.GetHostConnectionIdByGameId(gameId);
                await Clients.Client(hostConnectionId).DisconnectPlayer();
                await service.RemovePlayerByConnectionId(Context.ConnectionId);
            }
            else
            {
                await Clients.Group(service.GetGameIdByHostConnectionId(Context.ConnectionId)).GetSessionClosedScreen();
                await service.RemoveGameSession(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
