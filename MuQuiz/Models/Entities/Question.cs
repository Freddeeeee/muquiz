using System;
using System.Collections.Generic;

namespace MuQuiz.Models.Entities
{
    public partial class Question
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public string CorrectAnswer { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public int QuestionType { get; set; }

        public virtual Song Song { get; set; }
    }
}
