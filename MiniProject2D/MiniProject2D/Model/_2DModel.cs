using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.Input;

namespace MiniProject2D.Model
{
    class _2DModel
    {
        public enum Type
        {
            Texture = 0,
            Font = 1
        }

        private Type modelType;
        private Texture2D texture;
        private Rectangle rect;
        private Rectangle sourceRect;
        private SpriteFont spriteFont;
        private Vector2 pos;
        private Color color;
        private string text;

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        public Color CurrentColor
        {
            get { return color; }
            set { color = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public _2DModel(Texture2D texture, Rectangle rect, Color color, Rectangle sourceRect = default(Rectangle))
        {
            modelType = Type.Texture;
            this.Texture = texture;
            this.rect = rect;
            this.SourceRect = sourceRect;
            this.CurrentColor = color;
        }

        public _2DModel(string text, SpriteFont spriteFont, Vector2 pos, Color color)
        {
            modelType = Type.Font;
            this.Text = text;
            this.spriteFont = spriteFont;
            this.CurrentColor = color;
            this.Pos = pos;
        }
        

        private void Render(SpriteBatch renderComponent)
        {
            if (modelType == Type.Font)
            {
                renderComponent.DrawString(spriteFont, Text, Pos, CurrentColor);
            }
            else
            {
                if (sourceRect == default(Rectangle))
                    renderComponent.Draw(Texture, rect, CurrentColor);
                else
                    renderComponent.Draw(Texture, rect, sourceRect, CurrentColor);
            }
        }

        public void Offset(int x, int y)
        {
            rect.Offset(x, y);
        }


        public static void Render(_2DModel[] models)
        {
            var renderComponent = Global.Instance.RenderComponent;
            renderComponent.Begin();
            foreach (var model in models)
            {
                model.Render(renderComponent);
            }

            MouseEvent.Instance.CursorModel.Render(renderComponent);

            renderComponent.End();
        }


        public static _2DModel Clone(_2DModel obj)
        {
            return new _2DModel(obj.Texture, obj.rect, obj.color, obj.sourceRect);
        }
    }
}
