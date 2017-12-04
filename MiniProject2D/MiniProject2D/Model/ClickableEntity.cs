using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.EventHandler;
using MiniProject2D.Input;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.Model
{
    class ClickableEntity : GameEntity
    {
        private Texture2D hoverSprite;
        private EventBoard.Event ev;

        public bool IsHover { get; private set; }

        public ClickableEntity(Texture2D sprite, Rectangle rect, Color color)
            : base(sprite, rect, color)
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
            var hoverBefore = IsHover;
            IsHover = Rect.Contains(MouseEvent.Instance.MousePosition);
            if (!IsHover) return;
            if (!hoverBefore)
                SoundManager.Instance.PlaySound(ResManager.Instance.HoverSound);
            if (MouseEvent.Instance.IsLeftClick)
                LeftClick();
        }

        public void LeftClick()
        {
            SoundManager.Instance.PlaySound(ResManager.Instance.ClickSound);
            EventBoard.Instance.AddEvent(ev);
        }

        public override void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            if (isDisabled)
                spriteBatch.Draw(Sprite, Rect, Color.Gray);
            else if (IsHover)
                spriteBatch.Draw(hoverSprite, Rect, CurrentColor);
            else
                base.Draw(spriteBatch);
        }
    }
}
