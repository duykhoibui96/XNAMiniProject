using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.EventHandler;

namespace MiniProject2D.Model
{
    class ClickableEntity: GameEntity
    {
        private Texture2D hoverSprite;
        private EventBoard.Event ev;

        public bool IsHover { get; private set; }

        public ClickableEntity(Texture2D sprite, Rectangle rect, Color color) : base(sprite, rect, color)
        {
        }

        public ClickableEntity(EventBoard.Event ev, Texture2D sprite, Texture2D hoverSprite, Rectangle rect, Color color)
            : base(sprite, rect, color)
        {
            this.ev = ev;
            this.hoverSprite = hoverSprite;
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            IsHover = Rect.Contains(mouseState.X, mouseState.Y);
        }

        public void LeftClick()
        {
            EventBoard.Instance.Ev = ev;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsHover)
                spriteBatch.Draw(hoverSprite,Rect,CurrentColor);
            else
                base.Draw(spriteBatch);
        }
    }
}
