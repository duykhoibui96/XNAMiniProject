﻿using System;
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
            Exit = 9,
            ApplySetting = 10,
            CancelSetting = 11,
            ApplySpriteToGame = 12
        }

        public static EventBoard Instance;
        private Stack<Event> events;

        public void AddEvent(Event ev)
        {
            events.Push(ev);
        }

        public Event GetEvent()
        {
            return events.Count > 0 ? events.Peek() : Event.None;
        }

        static EventBoard()
        {
            Instance = new EventBoard();
        }

        private EventBoard()
        {
            events = new Stack<Event>();
        }

        public void Finish()
        {
            events.Pop();
        }


    }
}
