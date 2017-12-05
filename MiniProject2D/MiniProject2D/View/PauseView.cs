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
        private ButtonEntity resume;
        private ButtonEntity playAgain;
        private ButtonEntity setting;
        private ButtonEntity returnToMenu;

        public PauseView()
            : base()
        {
            Type = ViewType.PauseView;
        }

        public override void Init()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            var pos = new Vector2(graphicsDevice.Viewport.Width / 2 - unit * 4, unit);
            resume = new ButtonEntity("RESUME", pos, EventBoard.Event.ResumeGame);
            pos.Y += unit * 3;
            playAgain = new ButtonEntity("PLAY AGAIN", pos, EventBoard.Event.StartGame);
            pos.Y += unit * 3;
            setting = new ButtonEntity("SETTING", pos, EventBoard.Event.OpenSettings);
            pos.Y += unit * 3;
            returnToMenu = new ButtonEntity("RETURN TO MENU", pos, EventBoard.Event.ReturnToMenu);
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;
            resume.Update(gameTime);
            playAgain.Update(gameTime);
            setting.Update(gameTime);
            returnToMenu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = Mode == ViewMode.DISABLED;
            resume.Draw(spriteBatch, isDisabled);
            playAgain.Draw(spriteBatch, isDisabled);
            setting.Draw(spriteBatch, isDisabled);
            returnToMenu.Draw(spriteBatch, isDisabled);
        }
    }
}
