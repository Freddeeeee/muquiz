using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class HostService
    {
        public string GenerateGameId()
        {
            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string gameId = "";
            Random rand = new Random();

            for (int i = 0; i < 4; i++)
            {
                gameId += letters[rand.Next(0, letters.Length)];
            }

            return gameId;
        }

    }
}
