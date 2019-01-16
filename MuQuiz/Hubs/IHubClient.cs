using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Hubs
{
    public interface IHubClient
    {
        Task ReceiveSong(string song);
        Task ReceiveAnswer(string connectionId, bool correctAnswer, string name, string avatarCode);
        Task ReceiveConnectedPlayerName(string name, string avatarCode);
        Task GetWaitingScreen(string avatarCode);
        Task GetFinalPosition(int position, int score);
        Task GetSessionClosedScreen();
        Task DisconnectPlayer();
        Task ConfirmJoined();
    }
}
