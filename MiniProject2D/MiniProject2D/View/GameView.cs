using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.View
{
    abstract class GameView
    {
        public enum ViewMode
        {
            CURRENT = 0,
            DISABLED = 1,
            INVISIBLE = 2
        }

        public enum ViewType
        {
            MenuView = 0,
            PlayingView = 1,
            PauseView = 2,
            WinnerView = 3,
            LoserView = 4,
            SettingView = 5
        }

        protected ViewMode mode = ViewMode.CURRENT;
        protected ViewType type = ViewType.MenuView;

        public virtual ViewMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public ViewType Type
        {
            get { return type; }
            set { type = value; }
        }

        public abstract void Init();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
