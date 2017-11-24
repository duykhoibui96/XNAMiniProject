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
    class WinnerView: GameView
    {
        private ClickableEntity resetMatch;
        private ClickableEntity returnToMenu;
        private Vector2 notifyPos;
        private SpriteFont notifyFont;
        public WinnerView(Game game) : base(ViewType.Win)
        {
            notifyFont = ResManager.Instance.NotifyFont;
            notifyPos = new Vector2(game.GraphicsDevice.Viewport.Width / 2, Configuration.Unit * 2);
            var rect = new Rectangle(Configuration.Unit * 8, Configuration.Unit * 4, Configuration.Unit * 7,
               Configuration.Unit * 2);
            resetMatch = new ClickableEntity(EventBoard.Event.StartGame, ResManager.Instance.ResetMatch, ResManager.Instance.ResetMatchHover, rect, Color.White);
            rect.Y += Configuration.Unit * 2;
            rect.X -= Configuration.Unit * 2;
            rect.Width += Configuration.Unit * 4;
            returnToMenu = new ClickableEntity(EventBoard.Event.ReturnToMenu, ResManager.Instance.ReturnToMenu, ResManager.Instance.ReturnToMenuHover, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible || !isEnabled) return;
            resetMatch.Update(gameTime);
            returnToMenu.Update(gameTime);
            if (!UserInput.Instance.IsLeftClick) return;
            if (returnToMenu.IsHover)
                returnToMenu.LeftClick();
            else if (resetMatch.IsHover)
                resetMatch.LeftClick();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            string output = "YOU LOSE!";
            // Find the center of the string
            var FontOrigin = notifyFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(notifyFont, output, notifyPos, Color.Blue,
                0, FontOrigin, 3.0f, SpriteEffects.None, 0.5f);
            resetMatch.Draw(spriteBatch);
            returnToMenu.Draw(spriteBatch);
        }
    }
}
