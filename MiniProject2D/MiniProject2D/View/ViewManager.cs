using System;
using System.Collections.Generic;
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
        private GameView currentView; 
        private List<GameView> disabledViews;

        public ViewManager()
        {
            currentView = GetView(GameView.ViewType.MenuView);
            disabledViews = new List<GameView>();
        }


        public void Update(GameTime gameTime)
        {
            CheckEvent();
            currentView.Update(gameTime);
        }

        private void CheckEvent()
        {
            var ev = EventBoard.Instance.GetEvent();
            var eventCatch = true;

            switch (ev)
            {
                case EventBoard.Event.StartGame:
                    disabledViews.Clear();
                    currentView = GetView(GameView.ViewType.PlayingView);
                    break;
                case EventBoard.Event.OpenSettings:
                    currentView.Mode = GameView.ViewMode.DISABLED;
                    disabledViews.Add(currentView);
                    currentView = GetView(GameView.ViewType.SettingView);
                    break;
                case EventBoard.Event.CloseSettings:
                    currentView = disabledViews.Last();
                    currentView.Mode = GameView.ViewMode.CURRENT;
                    disabledViews.Remove(disabledViews.Last());
                    break;
                case EventBoard.Event.ReturnToMenu:
                    disabledViews.Clear();
                    currentView = GetView(GameView.ViewType.MenuView);
                    break;
                case EventBoard.Event.ShowResult:
                    disabledViews.Add(currentView);
                    currentView = GetView(PlayerRecord.Instance.IsWon ? GameView.ViewType.WinnerView : GameView.ViewType.LoserView);
                    break;
                default:
                    eventCatch = false;
                    break;

            }

            if (eventCatch)
            {
                EventBoard.Instance.Finish();
            }

        }

        private GameView GetView(GameView.ViewType viewType)
        {
            GameView view = null;
            switch (viewType)
            {
                case GameView.ViewType.MenuView:
                    view = new MenuView();
                    break;
                case GameView.ViewType.PlayingView:
                    view = new PlayingView();
                    break;
                case GameView.ViewType.SettingView:
                    view = new SettingView();
                    break;
                case GameView.ViewType.WinnerView:
                    view = new WinnerView();
                    break;
                case GameView.ViewType.LoserView:
                    view = new LoserView();
                    break;
            }

            if (view != null)
            {
                view.Init();

            }

            return view;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var disabledView in disabledViews)
            {
                disabledView.Draw(spriteBatch);
            }
            currentView.Draw(spriteBatch);
        }
    }
}
