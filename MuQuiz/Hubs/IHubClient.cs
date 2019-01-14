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
        Task GetWaitingScreen();
        Task GetFinalPosition(int position);
        Task GetSessionClosedScreen();
        Task DisconnectPlayer();
        Task ConfirmJoined();
    }
}
