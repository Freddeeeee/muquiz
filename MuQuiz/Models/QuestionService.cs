using Microsoft.Extensions.Configuration;
using MuQuiz.Models.Entities;
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

        public QuestionService(MuquizContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public string[] GetQuestionsForId(int id)
        {
            Random random = new Random(id);

            var questionInfo = context.Question
                .Where(q => q.SongId == id)
                .Select(o => new List<string>
                {
                    o.CorrectAnswer,
                    o.Answer1,
                    o.Answer2,
                    o.Answer3
                })
                .Single()
                .ToList();

            return questionInfo.OrderBy(o => random.Next()).ToArray();
        }

        public List<int> GetSongIds()
        {
            return context
                .Song.Select(s => s.Id)
                .OrderBy(s => Guid.NewGuid())
                .Take(configuration.GetSection("NumberOfSongs").Get<int>())
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
