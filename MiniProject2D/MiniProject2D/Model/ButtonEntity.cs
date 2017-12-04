using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.EventHandler;
using MiniProject2D.Input;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.Model
{
    class ButtonEntity : GameEntity
    {
        private string text;
        private Vector2 vector;
        private Rectangle lightRect;
        private bool isHover;
        private EventBoard.Event ev;

        public ButtonEntity(string text, Vector2 vector, Color color, EventBoard.Event ev)
            : base()
        {
            this.text = text;
            this.vector = vector;
            this.ev = ev;

            var textSize = ResManager.Instance.NotifyFont.MeasureString(text);
            Rect = new Rectangle((int)vector.X, (int)vector.Y, (int)textSize.X, (int)textSize.Y);
            lightRect = Rect;
            lightRect.Offset(-50,0);
            lightRect.Width += 100;
            CurrentColor = color;
        }



        public override void Update(GameTime gameTime)
        {
            var hoverBefore = isHover;
            isHover = Rect.Contains(MouseEvent.Instance.MousePosition);
            if (!isHover) return;
            if (!hoverBefore)
                SoundManager.Instance.PlaySound(ResManager.Instance.HoverSound);
            if (MouseEvent.Instance.IsLeftClick)
            {
                SoundManager.Instance.PlaySound(ResManager.Instance.ClickSound);
                EventBoard.Instance.AddEvent(ev);
            }
               

        }

        public override void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            if (isDisabled)
            {
                spriteBatch.DrawString(ResManager.Instance.NotifyFont, text, vector, Color.Gray);
            }
            else
            {
                spriteBatch.DrawString(ResManager.Instance.NotifyFont, text, vector, CurrentColor);
                if (isHover)
                    spriteBatch.Draw(ResManager.Instance.Light, lightRect, Color.AliceBlue);
            }
        }
    }
}
