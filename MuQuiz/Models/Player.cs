using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class Player
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string GameId { get; set; }
    }
}
