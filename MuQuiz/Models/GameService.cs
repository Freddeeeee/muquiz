using MuQuiz.Models.Entities;
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

        public async Task InitializeSession(string gameId)
        {
            await muquizContext.GameSession.AddAsync(new GameSession { GameId = gameId, IsPlaying = false });
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

        public void AddPlayer(string connectionId, string name, string gameId)
        {
            Players.Add(new Player
            {
                ConnectionId = connectionId,
                Name = name,
                GameId = gameId
            });
        }

        public Player[] GetAllPlayers(string gameId)
        {
            return Players
                .Where(p => p.GameId == gameId)
                .OrderByDescending(p => p.Score)
                .ToArray();
        }

        public void EvaluateAnswer(string connectionId, string answer)
        {
            Players
                .SingleOrDefault(p => p.ConnectionId == connectionId)
                .Score++;

        }
    }
}
