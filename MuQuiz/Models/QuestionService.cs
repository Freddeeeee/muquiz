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

        public QuestionService(MuquizContext context)
        {
            this.context = context;
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
