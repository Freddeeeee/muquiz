using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Hubs
{
    public interface IHubClient
    {
        Task ReceiveSong(string song);
        Task ReceiveAnswer(string answer);
        Task ReceiveConnectedPlayerName(string name);
        Task GetWaitingScreen();
        Task GetFinalPosition(int position);
    }
}
