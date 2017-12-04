using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class WinnerView: GameView
    {
        private ClickableEntity resetMatch;
        private ClickableEntity returnToMenu;
        private Vector2 notifyPos;
        private SpriteFont notifyFont;

        public WinnerView() : base()
        {
            Type = ViewType.LoserView;
            SoundManager.Instance.PlaySound(ResManager.Instance.WinSound);
        }

        public override void Init(GraphicsDevice graphicsDevice)
        {
            var unit = Configuration.Unit;
            notifyFont = ResManager.Instance.NotifyFont;
            notifyPos = new Vector2(graphicsDevice.Viewport.Width / 2, unit * 2);
            var rect = new Rectangle(unit * 8, unit * 4, unit * 7,
               unit * 2);
            resetMatch = new ClickableEntity(EventBoard.Event.StartGame, ResManager.Instance.ResetMatch, ResManager.Instance.ResetMatchHover, rect, Color.White);
            rect.Y += unit * 2;
            rect.X -= unit * 2;
            rect.Width += unit * 4;
            returnToMenu = new ClickableEntity(EventBoard.Event.ReturnToMenu, ResManager.Instance.ReturnToMenu, ResManager.Instance.ReturnToMenuHover, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != GameView.ViewMode.CURRENT) return;
            resetMatch.Update(gameTime);
            returnToMenu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == GameView.ViewMode.INVISIBLE) return;
            string output = "YOU WIN!";
            // Find the center of the string
            var FontOrigin = notifyFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(notifyFont, output, notifyPos, Color.Yellow,
                0, FontOrigin, 3.0f, SpriteEffects.None, 0.5f);
            resetMatch.Draw(spriteBatch);
            returnToMenu.Draw(spriteBatch);
        }
    }
}
