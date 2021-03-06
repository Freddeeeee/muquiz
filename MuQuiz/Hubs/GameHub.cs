﻿using Microsoft.AspNetCore.SignalR;
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

        public async Task AddToGroup(string gameId, string name, string avatarCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await service.AddPlayer(Context.ConnectionId, name, gameId, avatarCode);
            await SendNameToHost(gameId, name, avatarCode);
        }

        public async Task SendNameToHost(string gameId, string name, string avatarCode)
        {
            var host = await service.GetHostConnectionIdByGameId(gameId);
            await Clients.Client(host).ReceiveConnectedPlayerName(name, avatarCode);
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

        public async Task SendAnswer(int isCorrectAnswerInt, string gameId)
        {
            var connectionId = Context.ConnectionId;
            var hostConnectionId = await service.GetHostConnectionIdByGameId(gameId);
            bool isCorrectAnswer = isCorrectAnswerInt == 1;

            var playerInfo = await service.GetPlayerByConnectionId(connectionId);
            var name = playerInfo.Name;
            var avatarCode = playerInfo.AvatarCode;

            await Clients.Client(hostConnectionId).ReceiveAnswer(connectionId, isCorrectAnswer, name, avatarCode);
            await Clients.Client(connectionId).GetWaitingScreen(avatarCode);
        }

        public async Task UpdateScore(string connectionId, int position)
        {
            await service.UpdateScore(connectionId, position);
        }

        public async Task SendToWaitingScreen(string gameId)
        {
            var players = await service.GetAllPlayers(gameId);

            foreach (var player in players)
            {
                await Clients.Client(player.ConnectionId).GetWaitingScreen(player.AvatarCode);
            }
        }

        public async Task SendToFinalPosition(string gameId)
        {
            var players = await service.GetAllPlayers(gameId);

            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                await Clients.Client(player.ConnectionId).GetFinalPosition(i + 1, player.Score);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (await service.IsPlayer(Context.ConnectionId))
            {
                var gameId = await service.GetGameIdByHostConnectionId(Context.ConnectionId);
                var hostConnectionId = await service.GetHostConnectionIdByGameId(gameId);
                await Clients.Client(hostConnectionId).DisconnectPlayer();
                await service.RemovePlayerByConnectionId(Context.ConnectionId);
            }
            else
            {
                await Clients.Group(await service.GetGameIdByHostConnectionId(Context.ConnectionId)).GetSessionClosedScreen();
                await service.RemoveGameSession(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
