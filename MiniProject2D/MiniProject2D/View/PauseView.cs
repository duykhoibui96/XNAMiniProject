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

namespace MiniProject2D.View
{
    class PauseView : GameView
    {
        private ClickableEntity resume;
        private ClickableEntity resetMatch;
        private ClickableEntity settings;
        private ClickableEntity returnToMenu;

        public PauseView()
            : base()
        {
            Type = ViewType.PauseView;
        }

        public override void Init(GraphicsDevice graphicsDevice)
        {
            var unit = Configuration.Unit;
            var rect = new Rectangle(unit * 10, unit * 2, unit * 5,
                unit * 2);
            resume = new ClickableEntity(EventBoard.Event.ResumeGame, ResManager.Instance.Resume, ResManager.Instance.ResumeHover, rect, Color.White);
            rect.Y += unit * 2;
            rect.X -= 20;
            rect.Width += 40;
            settings = new ClickableEntity(EventBoard.Event.OpenSettings, ResManager.Instance.Settings, ResManager.Instance.SettingsHover, rect, Color.White);
            rect.Y += unit * 2;
            rect.X -= 30;
            rect.Width += 60;
            resetMatch = new ClickableEntity(EventBoard.Event.StartGame, ResManager.Instance.ResetMatch, ResManager.Instance.ResetMatchHover, rect, Color.White);
            rect.Y += unit * 2;
            rect.X -= unit * 2;
            rect.Width += unit * 4;
            returnToMenu = new ClickableEntity(EventBoard.Event.ReturnToMenu, ResManager.Instance.ReturnToMenu, ResManager.Instance.ReturnToMenuHover, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;
            resume.Update(gameTime);
            resetMatch.Update(gameTime);
            settings.Update(gameTime);
            returnToMenu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = Mode == ViewMode.DISABLED;
            resume.Draw(spriteBatch, isDisabled);
            settings.Draw(spriteBatch, isDisabled);
            resetMatch.Draw(spriteBatch, isDisabled);
            returnToMenu.Draw(spriteBatch, isDisabled);
        }
    }
}
