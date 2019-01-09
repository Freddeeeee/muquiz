using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class QuestionService
    {
        static List<int> songIds = new List<int> { 1, 2, 3 };
        static List<QuestionModel> questions = new List<QuestionModel> {
            new QuestionModel
            {
                SongId = 1,
                Alternatives = new string[]{"a1", "a2", "a3", "a4"}
            },
            new QuestionModel
            {
                SongId = 2,
                Alternatives = new string[]{"b1", "b2", "b3", "b4"}
            },
            new QuestionModel
            {
                SongId = 3,
                Alternatives = new string[]{"c1", "c2", "c3", "c4"}
            }
        };
        
        public string[] GetQuestionsForId(int id)
        {
            return questions
                .Single(q => q.SongId == id).Alternatives;
        }

        public List<int> GetSongIds()
        {
            return songIds;
        }
    }
}
