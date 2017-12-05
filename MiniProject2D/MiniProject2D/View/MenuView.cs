using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class MenuView : GameView
    {
        private BackgroundEntity background;
        private ButtonEntity newGame;
        private ButtonEntity setting;
        private ButtonEntity exit;

        public MenuView() : base()
        {
            Type = ViewType.MenuView;
            SoundManager.Instance.PlayMusic(ResManager.Instance.MenuMusic);
        }

        public override void Init()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            background = new BackgroundEntity(ResManager.Instance.MenuBackground, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            var pos = new Vector2(unit*3, unit*2);
            newGame = new ButtonEntity("NEW GAME", pos, EventBoard.Event.StartGame);
            pos.Y += unit * 3;
            setting = new ButtonEntity("SETTING", pos, EventBoard.Event.OpenSettings);
            pos.Y += unit * 3;
            exit = new ButtonEntity("EXIT", pos, EventBoard.Event.Exit);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;
            newGame.Update(gameTime);
            setting.Update(gameTime);
            exit.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = Mode == ViewMode.DISABLED;
            background.Draw(spriteBatch,isDisabled);
            setting.Draw(spriteBatch,isDisabled);
            newGame.Draw(spriteBatch,isDisabled);
            exit.Draw(spriteBatch,isDisabled);
        }
    }
}
