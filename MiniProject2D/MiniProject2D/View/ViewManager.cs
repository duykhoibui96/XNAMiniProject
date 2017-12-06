using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.GameComponent;
using MiniProject2D.Model;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class ViewManager
    {
        private GameView invisibleView;
        private GameView current;
        private Rectangle container;

        public ViewManager()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            container = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            current = new MenuView();
            current.Init(container);
        }

        public void Update(GameTime gameTime)
        {
            HandledEvent();
            current.Update(gameTime);
        }

        private void HandledEvent()
        {
            var ev = EventBoard.Instance.GetEvent();
            var eventHandled = true;
            var isInit = true;

            switch (ev)
            {
                case EventBoard.Event.StartGame:
                    current = new MainView();
                    break;
                case EventBoard.Event.OpenSettings:
                    invisibleView = current;
                    current = new SettingView();
                    break;
                case EventBoard.Event.CloseSettings:
                    current = invisibleView;
                    isInit = false;
                    break;
                case EventBoard.Event.ReturnToMenu:
                    current = new MenuView();
                    break;
                default:
                    eventHandled = false;
                    break;
            }

            if (eventHandled)
            {
                if (isInit)
                    current.Init(container);
                EventBoard.Instance.Finish();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current.Draw(spriteBatch);
        }

    }
}
