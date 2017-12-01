using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.Model
{
    abstract class GameEntity
    {
        public Texture2D Sprite;
        public Rectangle Rect;
        public Color CurrentColor;
        public bool IsVisible = true;

        protected GameEntity(Texture2D sprite, Rectangle rect, Color color)
        {
            Sprite = sprite;
            Rect = rect;
            CurrentColor = color;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            if (!IsVisible) return;
            spriteBatch.Draw(Sprite, Rect, isDisabled ? Color.Gray : CurrentColor);
        }
    }
}
