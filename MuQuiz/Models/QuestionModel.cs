using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class QuestionModel
    {
        public int SongId { get; set; }
        public string[] Alternatives { get; set; }
    }
}
