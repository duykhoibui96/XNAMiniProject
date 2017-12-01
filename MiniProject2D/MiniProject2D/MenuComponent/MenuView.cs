using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.MenuComponent
{
    class MenuView : GameView
    {
        private BackgroundEntity background;
        private ClickableEntity newGame;
        private ClickableEntity exit;

        public MenuView(Game game)
            : base(ViewType.Menu)
        {
            background = new BackgroundEntity(ResManager.Instance.MenuBackground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
            var rect = new Rectangle(Configuration.Unit * 3, Configuration.Unit * 2, Configuration.Unit * 6,
               Configuration.Unit * 2);
            newGame = new ClickableEntity(EventBoard.Event.StartGame, ResManager.Instance.NewGame, ResManager.Instance.NewGameHover, rect, Color.White);
            rect.Y += Configuration.Unit * 2;
            rect.Width -= Configuration.Unit * 3;
            exit = new ClickableEntity(EventBoard.Event.Exit, ResManager.Instance.Exit, ResManager.Instance.ExitHover, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isEnabled || !IsVisible) return;
            newGame.Update(gameTime);
            exit.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            background.Draw(spriteBatch);
            newGame.Draw(spriteBatch);
            exit.Draw(spriteBatch);
        }
    }
}
