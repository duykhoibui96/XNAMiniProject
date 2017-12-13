using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.Entity
{
    class Button: _2DEntity
    {
        private _2DModel textModel;
        private _2DModel container;
        private _2DModel light;
        private EventBoard.Event ev;
        private bool isHover;

        public Button(string text, Point position, EventBoard.Event ev)
        {
            this.ev = ev;
            var spriteFont = ResManager.Instance.NotifyFont;
            var unit = Global.Instance.Unit;
            var containerRect = new Rectangle(position.X, position.Y, unit * 8, unit * 2);
            var textVector = spriteFont.MeasureString(text);
            var textPosition = new Vector2(position.X + (containerRect.Width - textVector.X) / 2,
                position.Y + (containerRect.Height - textVector.Y) / 2);
            container = new _2DModel(ResManager.Instance.ButtonContainer, containerRect, Color.White);
            light = new _2DModel(ResManager.Instance.Light, containerRect, Color.White);
            textModel = new _2DModel(text, spriteFont, textPosition, Color.Red);
        }

        public void Interact()
        {
            var mouseEvent = MouseEvent.Instance;
            var hoverBefore = isHover;
            isHover = container.Rect.Contains(mouseEvent.MousePosition);
            if (isHover)
            {
                if (!hoverBefore)
                    SoundManager.Instance.PlaySound(ResManager.Instance.HoverSound);
                if (mouseEvent.IsLeftClick)
                {
                    SoundManager.Instance.PlaySound(ResManager.Instance.ClickSound);
                    EventBoard.Instance.AddEvent(ev);
                }
            }
        }

        public override _2DModel[] Models
        {
            get
            {
                var modelList = new List<_2DModel>()
                {
                    container,
                    textModel
                };

                if (isHover)
                    modelList.Add(light);

                return modelList.ToArray();

            }
        }
    }
}
