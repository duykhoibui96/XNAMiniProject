using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.Entity;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.UI
{
    class MenuUI : GameUI
    {
        private _2DModel background;
        private _2DModel logo;
        private Button newGame;
        private Button setting;
        private Button exit;

        public MenuUI()
        {
            Type = UIType.Menu;
            SoundManager.Instance.PlayMusic(ResManager.Instance.MenuMusic);
            var unit = Global.Instance.Unit;
            var graphics = Global.Instance.Graphics;
            var width = graphics.Viewport.Width;
            var logoStartX = (width - unit * 13) / 2;
            var buttonStartX = (width - unit * 8) / 2;
            background = new _2DModel(ResManager.Instance.MenuBackground, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);
            logo = new _2DModel(ResManager.Instance.Logo, new Rectangle(logoStartX, unit, unit * 13, unit * 3), Color.White);
            newGame = new Button("NEW GAME", new Point(buttonStartX, unit * 5), EventBoard.Event.StartGame);
            setting = new Button("SETTING", new Point(buttonStartX, unit * 8), EventBoard.Event.OpenSetting);
            exit = new Button("EXIT", new Point(buttonStartX, unit * 11), EventBoard.Event.Exit);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            newGame.Interact();
            setting.Interact();
            exit.Interact();
        }

        public override void Render()
        {
            if (mode == ViewMode.INVISIBLE) return;
            var modelList = new List<_2DModel>()
            {
                background,
                logo
            };

            modelList.AddRange(newGame.Models);
            modelList.AddRange(setting.Models);
            modelList.AddRange(exit.Models);

            _2DModel.Render(modelList.ToArray());

        }

        protected override void HandleEvent()
        {

        }
    }
}
