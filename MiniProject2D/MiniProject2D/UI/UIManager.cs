using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Sound;
using MiniProject2D.UI;

namespace MiniProject2D.UI
{
    class UIManager
    {
        private GameUI invisibleUI;
        private GameUI current;
        private GameUI dialog;
        private Rectangle container;

        public UIManager()
        {
            var graphicsDevice = Global.Instance.Graphics;
            container = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            current = new MenuUI();

        }

        public void Update(GameTime gameTime)
        {
            HandledEvent();
            if (dialog != null)
                dialog.Update(gameTime);
            else
                current.Update(gameTime);
        }

        private void HandledEvent()
        {
            var ev = EventBoard.Instance.GetEvent();
            var eventHandled = true;

            switch (ev)
            {
                case EventBoard.Event.StartGame:
                    current = new MainUI();
                    break;
                case EventBoard.Event.OpenSetting:
                    invisibleUI = current;
                    current = new SettingUI();
                    break;
                case EventBoard.Event.CloseSetting:
                    current = invisibleUI;
                    break;
                case EventBoard.Event.ReturnToMenu:
                    current = new MenuUI();
                    break;
                case EventBoard.Event.ShowNotification:
                    dialog = new NotificationUI(EventBoard.Instance.NotifyText);
                    break;
                case EventBoard.Event.DismissDialog:
                    dialog = null;
                    break;
                default:
                    eventHandled = false;
                    break;
            }

            if (eventHandled)
            {
                EventBoard.Instance.Finish();
            }

        }

        public void Render()
        {
            current.Render();
            if (dialog != null)
                dialog.Render();
        }

    }
}
