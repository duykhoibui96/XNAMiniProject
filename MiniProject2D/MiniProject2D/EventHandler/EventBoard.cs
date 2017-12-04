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
        public Event CurrentEvent = Event.None;

        static EventBoard()
        {
            Instance = new EventBoard();
        }

        private EventBoard()
        {
        }

        public void Clear()
        {
            CurrentEvent = Event.None;
        }

    }
}
