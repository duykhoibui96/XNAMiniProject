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
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class MainView : GameView
    {

        private OptionView optionView;
        private GameView mainView;
        private Rectangle mainViewContainer;

        public MainView()
        {
            optionView = new OptionView();
            mainView = new PlayingView();
        }

        public override void Init(Rectangle viewContainer)
        {
            base.Init(viewContainer);

            var unit = Configuration.Unit;
            var optionViewContainer = viewContainer;
            mainViewContainer = viewContainer;

            optionViewContainer.Width = unit * 10;
            mainViewContainer.Offset(unit * 10, 0);
            mainViewContainer.Width = viewContainer.Width - optionViewContainer.Width;

            optionView.Init(optionViewContainer);
            mainView.Init(mainViewContainer);

        }

        public override void Update(GameTime gameTime)
        {
            if (Mode != ViewMode.CURRENT) return;

            HandleEvent();
            optionView.Update(gameTime);
            mainView.Update(gameTime);
        }

        private void HandleEvent()
        {
            var ev = EventBoard.Instance.GetEvent();
            var eventHandled = true;
            switch (ev)
            {
                case EventBoard.Event.ShowResult:
                    mainView = new WinnerView();
                    break;
                case EventBoard.Event.ResetGame:
                    mainView = new PlayingView();
                    break;
                default:
                    eventHandled = false;
                    break;
            }

            if (eventHandled)
            {
                mainView.Init(mainViewContainer);
                EventBoard.Instance.Finish();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            optionView.Draw(spriteBatch);
            mainView.Draw(spriteBatch);
        }
    }
}
