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
        private ClickableEntity newGame;
        private ClickableEntity settings;
        private ClickableEntity exit;

        public MenuView() : base()
        {
            Type = ViewType.MenuView;
            SoundManager.Instance.PlayMusic(ResManager.Instance.MenuMusic);
        }

        public override void Init(GraphicsDevice graphicsDevice)
        {
            var unit = Configuration.Unit;
            background = new BackgroundEntity(ResManager.Instance.MenuBackground, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            var rect = new Rectangle(unit * 3, unit * 2, unit * 6,
               unit * 2);
            newGame = new ClickableEntity(EventBoard.Event.StartGame, ResManager.Instance.NewGame, ResManager.Instance.NewGameHover, rect, Color.White);
            rect.Y += unit * 2;
            settings = new ClickableEntity(EventBoard.Event.OpenSettings, ResManager.Instance.Settings, ResManager.Instance.SettingsHover, rect, Color.White);
            rect.Y += unit * 2;
            rect.Width -= unit * 3;
            exit = new ClickableEntity(EventBoard.Event.Exit, ResManager.Instance.Exit, ResManager.Instance.ExitHover, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;
            newGame.Update(gameTime);
            settings.Update(gameTime);
            exit.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = Mode == ViewMode.DISABLED;
            background.Draw(spriteBatch,isDisabled);
            settings.Draw(spriteBatch,isDisabled);
            newGame.Draw(spriteBatch,isDisabled);
            exit.Draw(spriteBatch,isDisabled);
        }
    }
}
