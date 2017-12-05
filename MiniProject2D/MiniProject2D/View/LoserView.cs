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
    class LoserView : GameView
    {
        private ButtonEntity playAgain;
        private ButtonEntity returnToMenu;
        private Vector2 notifyPos;
        private SpriteFont notifyFont;

        public LoserView() : base()
        {
            Type = ViewType.LoserView;
            SoundManager.Instance.PlaySound(ResManager.Instance.LoseSound);
        }

        public override void Init()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            notifyFont = ResManager.Instance.NotifyFont;
            notifyPos = new Vector2(graphicsDevice.Viewport.Width / 2, unit * 2);
            var pos = new Vector2(graphicsDevice.Viewport.Width / 2 - unit * 4, unit * 4);
            playAgain = new ButtonEntity("PLAY AGAIN", pos, EventBoard.Event.StartGame);
            pos.Y += unit * 3;
            returnToMenu = new ButtonEntity("RETURN TO MENU", pos, EventBoard.Event.ReturnToMenu);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;
            playAgain.Update(gameTime);
            returnToMenu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            string output = "YOU LOSE!";
            // Find the center of the string
            var FontOrigin = notifyFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(notifyFont, output, notifyPos, Color.Blue,
                0, FontOrigin, 3.0f, SpriteEffects.None, 0.5f);
            playAgain.Draw(spriteBatch);
            returnToMenu.Draw(spriteBatch);
        }
    }
}
