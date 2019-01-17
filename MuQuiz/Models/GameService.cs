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
        private readonly MuquizContext context;
        private Random rand = new Random();

        public GameService(MuquizContext context)
        {
            this.context = context;
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
            await context.GameSession.AddAsync(new GameSession { GameId = gameId, IsPlaying = false, HostConnectionId = connectionId });
            await context.SaveChangesAsync();
        }

        public async Task SetIsPlaying(string gameId, bool isPlaying)
        {
            context.GameSession.Single(g => g.GameId == gameId).IsPlaying = isPlaying;
            await context.SaveChangesAsync();
        }

        public async Task StartPlaying(string gameId)
        {
            await SetIsPlaying(gameId, true);
        }

        public async Task StopPlaying(string gameId)
        {
            await SetIsPlaying(gameId, false);
            var playersInThisSession = await context.Player
                .Where(p => p.GameSession.GameId == gameId)
                .ToListAsync();
            playersInThisSession.ForEach(p => p.Score = 0);
            await context.SaveChangesAsync();
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
            if (await context.GameSession.AnyAsync(g => g.GameId == gameId))
            {
                var result = await context.GameSession.AsNoTracking().SingleAsync(g => g.GameId == gameId);
                return !result.IsPlaying;
            }
            return false;
        }

        public async Task AddPlayer(string connectionId, string name, string gameId, string avatarCode)
        {
            var gameSession = await context.GameSession.AsNoTracking().SingleOrDefaultAsync(g => g.GameId == gameId);
            await context.Player.AddAsync(new Player
            {
                Name = name,
                ConnectionId = connectionId,
                Score = 0,
                GameSessionId = gameSession.Id,
                AvatarCode = avatarCode
            });
            await context.SaveChangesAsync();
        }

        internal async Task<bool> IsPlayer(string connectionId)
        {
            return await context.Player.AsNoTracking().AnyAsync(p => p.ConnectionId == connectionId);
        }

        public async Task<Player[]> GetAllPlayers(string gameId)
        {
            return await context.Player.AsNoTracking().Where(p => p.GameSession.GameId == gameId).OrderByDescending(p => p.Score).ToArrayAsync();
        }

        public async Task<Player[]> GetTopPlayers(string gameId, int numberOfPlayers)
        {
            return await muquizContext.Player.Where(p => p.GameSession.GameId == gameId).OrderByDescending(p => p.Score).Take(numberOfPlayers).ToArrayAsync();
        }

        public async Task<Player> GetPlayerByConnectionId(string connectionId)
        {
            return await context.Player.AsNoTracking().SingleOrDefaultAsync(s => s.ConnectionId == connectionId);
        }

        public async Task<string> GetHostConnectionIdByGameId(string gameId)
        {
            var result = await context.GameSession.AsNoTracking().SingleAsync(s => s.GameId == gameId);
            return result.HostConnectionId;
        }

        public async Task<string> GetGameIdByConnectionId(string connectionId)
        {
            var playerInfo = await context.Player.AsNoTracking().SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
            var gameSessionInfo = await context.GameSession.AsNoTracking().SingleOrDefaultAsync(g => g.Id == playerInfo.GameSessionId);

            return gameSessionInfo.GameId;
        }

        internal async Task<string> GetGameIdByHostConnectionId(string connectionId)
        {
            var result = await context.GameSession.AsNoTracking().SingleOrDefaultAsync(g => g.HostConnectionId == connectionId);
            return result.GameId;
        }

        internal async Task RemovePlayerByConnectionId(string connectionId)
        {
            var playerToRemove = await context.Player.SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
            context.Player.Remove(playerToRemove);
            await context.SaveChangesAsync();
        }

        internal async Task RemoveGameSession(string connectionId)
        {
            var playersToRemove = await context.Player.Where(p => p.GameSession.HostConnectionId == connectionId).ToListAsync();
            context.Player.RemoveRange(playersToRemove);

            var gameSessionToRemove = await context.GameSession.SingleOrDefaultAsync(g => g.HostConnectionId == connectionId);
            context.GameSession.Remove(gameSessionToRemove);

            await context.SaveChangesAsync();
        }

        internal async Task UpdateScore(string connectionId, int count)
        {
            if (count > 3)
                count = 4;

            var player = await context.Player.SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
            player.Score += 1000 + 100 * (4 - count);
            await context.SaveChangesAsync();
        }
    }
}
