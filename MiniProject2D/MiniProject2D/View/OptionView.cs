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
    class OptionView: GameView
    {

        private BackgroundEntity logo;
        private BackgroundEntity menuContainer;
        private ButtonEntity playAgain;
        private ButtonEntity setting;
        private ButtonEntity returnToMenu;


        public override void Init(Rectangle container)
        {
            base.Init(container);
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            var pos = new Vector2(unit, unit);

            var menuTexture = new Texture2D(graphicsDevice, 1, 1);
            menuTexture.SetData(new Color[]
            {
                Color.Blue
            });

            menuContainer = new BackgroundEntity(menuTexture, viewContainer, Color.White);

            logo = new BackgroundEntity(ResManager.Instance.Logo, new Rectangle((int)pos.X, (int)pos.Y, unit * 8, unit * 2), Color.White);
            pos.Y += unit * 3;
            playAgain = new ButtonEntity("PLAY AGAIN", pos, EventBoard.Event.ResetGame);
            pos.Y += unit * 3;
            setting = new ButtonEntity("SETTING", pos, EventBoard.Event.OpenSettings);
            pos.Y += unit * 3;
            returnToMenu = new ButtonEntity("RETURN TO MENU", pos, EventBoard.Event.ReturnToMenu);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;

            playAgain.Update(gameTime);
            setting.Update(gameTime);
            returnToMenu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = mode == ViewMode.DISABLED;

            menuContainer.Draw(spriteBatch, isDisabled);
            logo.Draw(spriteBatch);
            playAgain.Draw(spriteBatch, isDisabled);
            setting.Draw(spriteBatch, isDisabled);
            returnToMenu.Draw(spriteBatch, isDisabled);
        }
    }
}
