using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.Model
{
    abstract class GameView
    {
        public enum ViewType
        {
            Menu = 0,
            Match = 1,
            Pause = 2,
            Settings = 3,
            Win = 4,
            Lose = 5
        }

        public ViewType Type;
        public bool IsVisible = true;
        protected bool isEnabled = true;

        protected GameView(ViewType viewType)
        {
            Type = viewType;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void SetEnabled(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }

    }
}
