using System;
using System.Collections.Generic;

namespace MuQuiz.Models.Entities
{
    public partial class Song
    {
        public Song()
        {
            Question = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string SpotifyId { get; set; }
        public string SongName { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Question> Question { get; set; }
    }
}
