using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniProject2D.GameComponent
{
    class PlayerRecord
    {
        public bool IsWon { get; private set; }
        public int Score { get; private set; }

        private PlayerRecord()
        {
        }

        public static PlayerRecord Instance;

        static PlayerRecord()
        {
            Instance = new PlayerRecord();
        }

        public void SetResult(bool isWon, int score = 0)
        {
            IsWon = isWon;
            Score = score < 0 ? 0 : score;
        }
    }
}
