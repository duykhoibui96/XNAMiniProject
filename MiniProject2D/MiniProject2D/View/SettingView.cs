using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.View
{
    class SettingView : GameView
    {
        private BackgroundEntity container;
        private ButtonEntity ok;
        private ButtonEntity apply;
        private ButtonEntity cancel;

        private Vector2 settingPosition;

        public SettingView()
            : base()
        {
            Type = ViewType.SettingView;
        }

        public override void Init(GraphicsDevice graphicsDevice)
        {
            int width = graphicsDevice.Viewport.Width - 300, height = graphicsDevice.Viewport.Height - 200;
            int startX = 150, startY = 100;
            int middle = startX + width / 2;

            container = new BackgroundEntity(ResManager.Instance.Dialog, new Rectangle(startX, startY, width, height), Color.White);
            settingPosition = new Vector2(middle - ResManager.Instance.NotifyFont.MeasureString("SETTING").X / 2, 150);
            apply = new ButtonEntity("APPLY", new Vector2(startX + width / 4, startY + height - 100), Color.Yellow, EventBoard.Event.CloseSettings);
            ok = new ButtonEntity("OK", new Vector2(startX + width / 2, startY + height - 100), Color.Yellow, EventBoard.Event.CloseSettings);
            cancel = new ButtonEntity("CANCEL", new Vector2(startX + 2 * width / 3, startY + height - 100), Color.Yellow, EventBoard.Event.CloseSettings);
        }

        public override void Update(GameTime gameTime)
        {
            apply.Update(gameTime);
            ok.Update(gameTime);
            cancel.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            container.Draw(spriteBatch);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "SETTING", settingPosition, Color.Red);
            apply.Draw(spriteBatch);
            ok.Draw(spriteBatch);
            cancel.Draw(spriteBatch);

        }
    }
}
