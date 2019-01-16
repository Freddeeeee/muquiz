using Microsoft.Extensions.Configuration;
using MuQuiz.Models.Entities;
using MuQuiz.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class QuestionService
    {
        private readonly MuquizContext context;
        private readonly IConfiguration configuration;
        private readonly SessionStorageService sessionStorage;

        public QuestionService(MuquizContext context, IConfiguration configuration, SessionStorageService sessionStorage)
        {
            this.context = context;
            this.configuration = configuration;
            this.sessionStorage = sessionStorage;
        }

        public AlternativesVM[] GetQuestionsForId(int id)
        {
            Random random = new Random(id);

            var questionInfo = context.Question
                .Where(q => q.SongId == id)
                .Select(o => new List<AlternativesVM>
                {
                    new AlternativesVM {Alternative = o.CorrectAnswer, IsRightAlt = 1 },
                    new AlternativesVM {Alternative = o.Answer1 },
                    new AlternativesVM {Alternative = o.Answer2 },
                    new AlternativesVM {Alternative = o.Answer3 },
                })
                .Single()
                .ToList();

            return questionInfo.OrderBy(o => random.Next()).ToArray();
        }

        public List<int> GetSongIds()
        {
            GameConfiguration config;

            if (string.IsNullOrEmpty(sessionStorage.GameConfiguration))
            {
                // set default
                config = new GameConfiguration { NumberOfSongs = configuration.GetSection("NumberOfSongs").Get<int>() };
            }
            else
            {
                config = JsonConvert.DeserializeObject<GameConfiguration>(sessionStorage.GameConfiguration);
            }

            return context
                .Song.Select(s => s.Id)
                .OrderBy(s => Guid.NewGuid())
                .Take(config.NumberOfSongs)
                .ToList();
        }

        public string GetSpotifyId(int id)
        {
            return context.Song
                .SingleOrDefault(s => s.Id == id)
                .SpotifyId;
        }
    }
}
