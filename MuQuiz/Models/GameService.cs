using Microsoft.EntityFrameworkCore;
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
        private Random rand = new Random();

        public GameService(MuquizContext muquizContext)
        {
            this.muquizContext = muquizContext;
        }

        public string GenerateGameId()
        {
            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string gameId = "";

            for (int i = 0; i < 4; i++)
            {
                gameId += letters[rand.Next(0, letters.Length)];
            }

            return gameId;
        }

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
            muquizContext.Player
                .Where(p => p.GameSession.GameId == gameId)
                .ToList()
                .ForEach(p => p.Score = 0);
            await muquizContext.SaveChangesAsync();
        }

        internal string GetRandomAvatar()
        {
            string[] avatarCodes = { "ant", "amphora", "avocado", "balloon", "banana", "basketball", "baguette_bread",
            "beer", "beetle", "birthday", "blowfish", "bike", "boat", "brain", "burrito", "broccoli", "bug", "cactus",
            "camel", "candy", "car", "carrot", "cat", "cherries", "cheese_wedge", "circus_tent", "coconut", "coffee", "cookie", "cow2",
            "crocodile", "crystal_ball", "cucumber", "crossed_swords", "deer", "dragon_face", "duck", "eagle", "earth_africa", "elephant",
            "exploding_head", "female-detective", "fire", "frog", "fries", "gorilla", "grapes", "hankey", "hedgehog",
            "honey_pot", "joystick", "kiwifruit", "leopard", "lizard", "mage", "monkey", "moon", "mountain", "motor_scooter",
            "owl", "palm_tree", "pig", "pizza", "potato", "ramen", "scooter", "sauropod", "snake", "snowflake", "spider_web",
            "squid", "sunflower", "taco", "tiger", "turtle", "volcano", "zebra_face", "8ball", "alarm_clock", "alien", "ambulance",
            "anchor", "apple", "art", "bacon", "bamboo", "baseball", "bat", "bee", "bird", "boar", "bomb", "boom", "boxing_glove",
            "bulb", "call_me_hand", "candle", "champagne", "chicken", "coffin", "corn", "crab", "crown", "cut_of_meat", "dark_sunglasses",
            "dog", "doughnut", "eggplant", "face_with_cowboy_hat", "fried_shrimp", "genie", "gun", "icecream", "hotdog", "hot_pepper"};

            return avatarCodes[rand.Next(avatarCodes.Length)];
        }

        public async Task<bool> SessionIsActive(string gameId)
        {
            if (await muquizContext.GameSession.AnyAsync(g => g.GameId == gameId))
            {
                var result = await muquizContext.GameSession.SingleAsync(g => g.GameId == gameId);
                return !result.IsPlaying;
            }
            return false;
        }

        public async Task AddPlayer(string connectionId, string name, string gameId, string avatarCode)
        {
            await muquizContext.Player.AddAsync(new Player
            {
                Name = name,
                ConnectionId = connectionId,
                Score = 0,
                GameSessionId = muquizContext.GameSession.SingleOrDefault(g => g.GameId == gameId).Id,
                AvatarCode = avatarCode
            });
            await muquizContext.SaveChangesAsync();
        }

        internal async Task<bool> IsPlayer(string connectionId)
        {
            return await muquizContext.Player.AnyAsync(p => p.ConnectionId == connectionId);
        }

        public async Task<Player[]> GetAllPlayers(string gameId)
        {
            return await muquizContext.Player.Where(p => p.GameSession.GameId == gameId).OrderByDescending(p => p.Score).ToArrayAsync();
        }

        public async Task<Player[]> GetTopPlayers(string gameId, int numberOfPlayers)
        {
            return await muquizContext.Player.Where(p => p.GameSession.GameId == gameId).OrderByDescending(p => p.Score).Take(numberOfPlayers).ToArrayAsync();
        }

        public async Task<Player> GetPlayerByConnectionId(string connectionId)
        {
            return await muquizContext.Player.SingleOrDefaultAsync(s => s.ConnectionId == connectionId);
        }

        public async Task<string> GetHostConnectionIdByGameId(string gameId)
        {
            var result = await muquizContext.GameSession.SingleAsync(s => s.GameId == gameId);
            return result.HostConnectionId;
        }

        public async Task<string> GetGameIdByConnectionId(string connectionId)
        {
            var playerInfo = await muquizContext.Player.SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
            var gameSessionInfo = await muquizContext.GameSession.SingleOrDefaultAsync(g => g.Id == playerInfo.GameSessionId);

            return gameSessionInfo.GameId;
        }

        internal async Task<string> GetGameIdByHostConnectionId(string connectionId)
        {
            var result = await muquizContext.GameSession.SingleOrDefaultAsync(g => g.HostConnectionId == connectionId);
            return result.GameId;
        }

        internal async Task RemovePlayerByConnectionId(string connectionId)
        {
            var playerToRemove = await muquizContext.Player.SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
            muquizContext.Player.Remove(playerToRemove);
            await muquizContext.SaveChangesAsync();
        }

        internal async Task RemoveGameSession(string connectionId)
        {
            var playersToRemove = await muquizContext.Player.Where(p => p.GameSession.HostConnectionId == connectionId).ToListAsync();
            muquizContext.Player.RemoveRange(playersToRemove);

            var gameSessionToRemove = await muquizContext.GameSession.SingleOrDefaultAsync(g => g.HostConnectionId == connectionId);
            muquizContext.GameSession.Remove(gameSessionToRemove);

            await muquizContext.SaveChangesAsync();
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
