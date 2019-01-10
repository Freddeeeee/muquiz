using MuQuiz.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class GameService
    {
        MuquizContext context;

        public GameService(MuquizContext context)
        {
            this.context = context;
        }

        public void AddPlayer(string connectionId, string name, string gameId)
        {
            context.Player.Add(new Player
            {
                Name = name,
                ConnectionId = connectionId,
                Score = 0,
                GameSessionId = context.GameSession.SingleOrDefault(g => g.GameId == gameId).Id
            });
            context.SaveChanges();
        }

        public Player[] GetAllPlayers(string gameId)
        {
            return context.Player
                .Where(p => p.GameSession.GameId == gameId)
                .OrderByDescending(p => p.Score)
                .ToArray();
        }

        public void EvaluateAnswer(string connectionId, string answer)
        {
            context.Player
                .SingleOrDefault(p => p.ConnectionId == connectionId)
                .Score++;

        }
    }
}
