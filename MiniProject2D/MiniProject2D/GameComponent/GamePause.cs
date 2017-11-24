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

namespace MiniProject2D.GameComponent
{
    class GamePause : GameView
    {
        private ClickableEntity resume;
        private ClickableEntity resetMatch;
        private ClickableEntity settings;
        private ClickableEntity returnToMenu;
        public GamePause(Game game)
            : base(ViewType.Pause)
        {
            var rect = new Rectangle(Configuration.Unit * 10, Configuration.Unit * 2, Configuration.Unit * 5,
                Configuration.Unit * 2);
            resume = new ClickableEntity(EventBoard.Event.ResumeGame, ResManager.Instance.Resume, ResManager.Instance.ResumeHover, rect, Color.White);
            rect.Y += Configuration.Unit*2;
            rect.X -= 20;
            rect.Width += 40;
            settings = new ClickableEntity(EventBoard.Event.OpenSettings, ResManager.Instance.Settings, ResManager.Instance.SettingsHover, rect, Color.White);
            rect.Y += Configuration.Unit * 2;
            rect.X -= 30;
            rect.Width += 60;
            resetMatch = new ClickableEntity(EventBoard.Event.StartGame, ResManager.Instance.ResetMatch, ResManager.Instance.ResetMatchHover, rect, Color.White);
            rect.Y += Configuration.Unit * 2;
            rect.X -= Configuration.Unit * 2;
            rect.Width += Configuration.Unit * 4;
            returnToMenu = new ClickableEntity(EventBoard.Event.ReturnToMenu, ResManager.Instance.ReturnToMenu, ResManager.Instance.ReturnToMenuHover, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible || !isEnabled) return;
            resume.Update(gameTime);
            resetMatch.Update(gameTime);
            settings.Update(gameTime);
            returnToMenu.Update(gameTime);
            if (!UserInput.Instance.IsLeftClick) return;
            if (resume.IsHover)
                resume.LeftClick();
            else if (settings.IsHover)
                settings.LeftClick();
            else if (returnToMenu.IsHover)
                returnToMenu.LeftClick();
            else if (resetMatch.IsHover)
                resetMatch.LeftClick();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            resume.Draw(spriteBatch);
            settings.Draw(spriteBatch);
            resetMatch.Draw(spriteBatch);
            returnToMenu.Draw(spriteBatch);
        }
    }
}
