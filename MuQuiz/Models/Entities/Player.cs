using System;
using System.Collections.Generic;

namespace MuQuiz.Models.Entities
{
    public partial class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public int Score { get; set; }
        public int GameSessionId { get; set; }

        public virtual GameSession GameSession { get; set; }
    }
}
