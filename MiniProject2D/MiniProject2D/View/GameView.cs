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
            MainView = 1,
            PlayingView = 2,
            PauseView = 3,
            WinnerView = 4,
            LoserView = 5,
            SettingView = 6
        }

        protected ViewMode mode = ViewMode.CURRENT;
        protected ViewType type = ViewType.MenuView;
        protected Rectangle viewContainer;

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

        public virtual void Init(Rectangle viewContainer)
        {
            this.viewContainer = viewContainer;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
