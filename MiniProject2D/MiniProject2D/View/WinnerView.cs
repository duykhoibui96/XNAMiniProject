using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.GameComponent;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class WinnerView : GameView
    {
        private BackgroundEntity container;

        private ButtonEntity playAgain;
        private ButtonEntity returnToMenu;
        private Vector2 notifyPos;
        private Vector2 scorePos;

        private PlayerInfo playerInfo;

        private int playerScore;

        public WinnerView()
            : base()
        {
            Type = ViewType.LoserView;
            SoundManager.Instance.PlaySound(ResManager.Instance.WinSound);
        }

        public override void Init()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            var containerTexture = new Texture2D(Setting.Instance.Graphics, 1, 1);
            containerTexture.SetData(new Color[]
            {
                Color.Green
            });

            container = new BackgroundEntity(containerTexture, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            var unit = Configuration.Unit;
            playerScore = PlayerRecord.Instance.Score;
            notifyPos = new Vector2(graphicsDevice.Viewport.Width / 2, unit * 2);
            scorePos = new Vector2(unit, unit * 4);
            var pos = new Vector2(graphicsDevice.Viewport.Width - unit * 10, unit * 4);
            playAgain = new ButtonEntity("PLAY AGAIN", pos, EventBoard.Event.StartGame);
            pos.Y += unit * 3;
            returnToMenu = new ButtonEntity("RETURN TO MENU", pos, EventBoard.Event.ReturnToMenu);

            playerInfo = new PlayerInfo();
            playerInfo.Init(unit, unit * 6);

        }

        public override void Update(GameTime gameTime)
        {
            if (mode != GameView.ViewMode.CURRENT) return;
            playAgain.Update(gameTime);
            returnToMenu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == GameView.ViewMode.INVISIBLE) return;
            container.Draw(spriteBatch);
            string output = "YOU WIN!";
            // Find the center of the string
            var FontOrigin = ResManager.Instance.NotifyFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, output, notifyPos, Color.Yellow,
                0, FontOrigin, 3.0f, SpriteEffects.None, 0.5f);

            output = "YOUR SCORE: " + playerScore;
            spriteBatch.DrawString(ResManager.Instance.SmallNotifyFont, output, scorePos, Color.WhiteSmoke);
            
            playerInfo.Draw(spriteBatch);

            playAgain.Draw(spriteBatch);
            returnToMenu.Draw(spriteBatch);
        }
    }
}
