﻿using MuQuiz.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class GameService
    {
        private readonly MuquizContext muquizContext;

        public GameService(MuquizContext muquizContext)
        {
            this.muquizContext = muquizContext;
        }

        static public List<Player> Players { get; set; } = new List<Player>();

        public async Task InitializeSession(string gameId, string connectionId)
        {
            await muquizContext.GameSession.AddAsync(new GameSession { GameId = gameId, IsPlaying = false, HostConnectionId = connectionId });
            await muquizContext.SaveChangesAsync();
        }

        public async Task SetIsPlaying(string gameId, bool isPlaying)
        {
            muquizContext.GameSession.Single(g => g.GameId == gameId).IsPlaying = isPlaying;
            await muquizContext.SaveChangesAsync();
        }

        public async Task StartPlaying(string gameId)
        {
            await SetIsPlaying(gameId, true);
        }

        public async Task StopPlaying(string gameId)
        {
            await SetIsPlaying(gameId, false);
        }

        public bool SessionIsActive(string gameId)
        {
            if (muquizContext.GameSession.Any(g => g.GameId == gameId))
                if (muquizContext.GameSession.Single(g => g.GameId == gameId).IsPlaying == false)
                    return true;
            return false;
        }

        public void AddPlayer(string connectionId, string name, string gameId)
        {
            muquizContext.Player.Add(new Player
            {
                Name = name,
                ConnectionId = connectionId,
                Score = 0,
                GameSessionId = muquizContext.GameSession.SingleOrDefault(g => g.GameId == gameId).Id
            });
            muquizContext.SaveChanges();
        }

        

        internal bool IsPlayer(string connectionId)
        {
            return muquizContext.Player.Count(p => p.ConnectionId == connectionId) > 0;
        }

        public Player[] GetAllPlayers(string gameId)
        {
            return muquizContext.Player
                .Where(p => p.GameSession.GameId == gameId)
                .OrderByDescending(p => p.Score)
                .ToArray();
        }

        internal void RemoveAllPlayers(string connectionId)
        {
            var playersToRemove = muquizContext.Player.Where(p => p.GameSession.HostConnectionId == connectionId);
            muquizContext.Player.RemoveRange(playersToRemove);
            muquizContext.SaveChanges();
        }

        public Player GetPlayerByConnectionId(string connectionId)
        {
            return muquizContext
                .Player
                .SingleOrDefault(s => s.ConnectionId == connectionId);
        }

        internal void RemovePlayerByConnectionId(string connectionId)
        {
            var playerToRemove = muquizContext.Player.SingleOrDefault(p => p.ConnectionId == connectionId);
            muquizContext.Player.Remove(playerToRemove);
            muquizContext.SaveChanges();
        }

        public string GetHostConnectionIdByGameId(string gameId)
        {
            return muquizContext.GameSession.Single(s => s.GameId == gameId).HostConnectionId;
        }

        internal string GetGameIdByConnectionId(string connectionId)
        {
            return muquizContext.GameSession.SingleOrDefault(g => g.HostConnectionId == connectionId).GameId;
        }

        internal void RemoveGameSession(string connectionId)
        {
            var gameSessionToRemove = muquizContext.GameSession.SingleOrDefault(g => g.HostConnectionId == connectionId);
            muquizContext.GameSession.Remove(gameSessionToRemove);
            muquizContext.SaveChanges();
        }

        public bool EvaluateAnswer(string connectionId, string answer)
        {
            return muquizContext.Question.Count(q => answer == q.CorrectAnswer) > 0;
        }

        internal async Task UpdateScore(string connectionId, int count)
        {
            if (count > 3)
                count = 4;

            muquizContext.Player
                    .SingleOrDefault(p => p.ConnectionId == connectionId)
                    .Score += 1000 + 100 * (4 - count);
            await muquizContext.SaveChangesAsync();
        }

    }
}
