using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class GameService
    {
        static public List<Player> Players { get; set; } = new List<Player>();

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
