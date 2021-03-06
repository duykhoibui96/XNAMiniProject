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
            ResetGame = 1,
            OpenSetting = 2,
            CloseSetting = 3,
            StartGame = 4,
            ReturnToMenu = 5,
            ShowResult = 6,
            Exit = 7,
            ApplySetting = 8,
            CancelSetting = 9,
            ApplySpriteToGame = 10,
            Collision = 11,
            SaveRecord = 12,
            ShowNotification = 13,
            DismissDialog = 14
        }

        public static EventBoard Instance;
        private Stack<Event> events;
        public string NotifyText { get; set; }

        public void AddEvent(Event ev)
        {
            if (events.Count > 0 && events.Peek() == ev) return;
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
