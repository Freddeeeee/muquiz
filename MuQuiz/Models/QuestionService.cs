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
            var questionInfo = context.Question
                .SingleOrDefault(q => q.SongId == id);

            return new string[] { questionInfo.CorrectAnswer, questionInfo.Answer1, questionInfo.Answer2, questionInfo.Answer3 };
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
