using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
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

        public ButtonEntity(string text, Vector2 vector, EventBoard.Event ev)
            : base()
        {
            this.text = text;
            this.vector = vector;
            this.ev = ev;

            var unit = Configuration.Unit;

            Rect = new Rectangle((int)vector.X, (int)vector.Y, unit * 8, unit * 2);
            var textSize = ResManager.Instance.NotifyFont.MeasureString(text);
            this.vector = new Vector2(vector.X + (Rect.Width - textSize.X) / 2, vector.Y + (Rect.Height - textSize.Y) / 2);
            lightRect = Rect;
            CurrentColor = Color.Red;

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
                spriteBatch.Draw(ResManager.Instance.ButtonContainer, Rect, Color.Gray);
                spriteBatch.DrawString(ResManager.Instance.NotifyFont, text, vector, Color.Gray);
            }
            else
            {
                spriteBatch.Draw(ResManager.Instance.ButtonContainer, Rect, Color.White);
                spriteBatch.DrawString(ResManager.Instance.NotifyFont, text, vector, CurrentColor);
                if (isHover)
                    spriteBatch.Draw(ResManager.Instance.Light, lightRect, Color.AliceBlue);
            }
        }
    }
}
