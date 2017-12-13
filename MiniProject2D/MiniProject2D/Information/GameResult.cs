using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using MiniProject2D.Entity;
using MiniProject2D.Model;

namespace MiniProject2D.Information
{
    class GameResult
    {
        public static GameResult Instance;

        private bool isWon;
        private int score;
        private string playerName;

        static GameResult()
        {
            Instance = new GameResult();
        }

        private GameResult()
        {
            playerName = "TONY";
            score = 1000;
        }

        public bool IsWon
        {
            get { return isWon; }
            set { isWon = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value <= 0 ? 0 : value; }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
    }
}
