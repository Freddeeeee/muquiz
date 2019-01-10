using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class HostGameVM
    {
        public string GameId { get; set; }
        public string SongIds { get; set; }
        public string SpotifyToken { get; set; }
    }
}
