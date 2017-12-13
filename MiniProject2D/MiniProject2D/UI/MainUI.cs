using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Entity;
using MiniProject2D.EventHandler;
using MiniProject2D.Information;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.UI
{
    class MainUI : GameUI
    {
        private enum State
        {
            Playing = 0,
            Win = 1,
            Lose = 2
        }

        private State state;

        private _2DModel logo;
        private _2DModel background;
        private Button playAgain;
        private Button setting;
        private Button returnToMenu;

        private MazeUI mazeUI;
        private WinnerUI winnerUI;
        private LoserUI loserUI;

        public MainUI()
        {
            SoundManager.Instance.PlayMusic(ResManager.Instance.GameMusic);
            state = State.Playing;
            var unit = Global.Instance.Unit;
            var graphics = Global.Instance.Graphics;
            mazeUI = new MazeUI();
            winnerUI = new WinnerUI();
            loserUI = new LoserUI();

            background = new _2DModel(ResManager.Instance.Ground, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);
            logo = new _2DModel(ResManager.Instance.Logo, new Rectangle(unit, unit, unit * 8, unit * 2), Color.White);

            playAgain = new Button("PLAY AGAIN", new Point(unit, unit * 4), EventBoard.Event.ResetGame);
            setting = new Button("SETTING", new Point(unit, unit * 7), EventBoard.Event.OpenSetting);
            returnToMenu = new Button("RETURN TO MENU", new Point(unit, unit * 10), EventBoard.Event.ReturnToMenu);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (state)
            {
                case State.Playing:
                    mazeUI.Update(gameTime);
                    break;
                case State.Win:
                    winnerUI.Update(gameTime);
                    break;
                case State.Lose:
                    break;
            }

            playAgain.Interact();
            setting.Interact();
            returnToMenu.Interact();

        }

        protected override  void HandleEvent()
        {
            var ev = EventBoard.Instance.GetEvent();
            var eventHandled = true;
            switch (ev)
            {
                case EventBoard.Event.ShowResult:
                    if (GameResult.Instance.IsWon)
                    {
                        winnerUI.UpdateInformation();
                        SoundManager.Instance.PlaySound(ResManager.Instance.WinSound);
                        state = State.Win;
                    }
                    else
                    {
                        loserUI.UpdateInformation();
                        SoundManager.Instance.PlaySound(ResManager.Instance.LoseSound);
                        state = State.Lose;
                    }
                    break;
                case EventBoard.Event.ResetGame:
                    state = State.Playing;
                    eventHandled = false;
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

        public override void Render()
        {
            var modelList = new List<_2DModel>();

            modelList.Add(background);
            modelList.Add(logo);
            modelList.AddRange(playAgain.Models);
            modelList.AddRange(setting.Models);
            modelList.AddRange(returnToMenu.Models);

            _2DModel.Render(modelList.ToArray());

            switch (state)
            {
                case State.Playing:
                    mazeUI.Render();
                    break;
                case State.Win:
                    winnerUI.Render();
                    break;
                case State.Lose:
                    loserUI.Render();
                    break;
            }

        }

    }
}
