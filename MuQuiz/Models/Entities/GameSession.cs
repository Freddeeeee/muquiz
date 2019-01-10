using System;
using System.Collections.Generic;

namespace MuQuiz.Models.Entities
{
    public partial class GameSession
    {
        public GameSession()
        {
            Player = new HashSet<Player>();
        }

        public int Id { get; set; }
        public string GameId { get; set; }
        public bool State { get; set; }

        public virtual ICollection<Player> Player { get; set; }
    }
}
