using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniProject2D.EventHandler
{
    internal class EventBoard
    {
        public enum Event
        {
            None = 0,
            PauseGame = 1,
            ResumeGame = 2,
            OpenSettings = 3,
            CloseSettings = 4,
            StartGame = 5,
            ReturnToMenu = 6,
            ShowResultsWhenWin = 7,
            ShowResultsWhenLose = 8,
            Exit = 9
        }

        public static EventBoard Instance;
        private Event ev;

        static EventBoard()
        {
            Instance = new EventBoard();
        }

        private EventBoard()
        {
        }

        public Event Ev
        {
            get
            {
                var ev = this.ev;
                this.ev = Event.None;
                return ev;
            }
            set { ev = value; }
        }
    }
}
