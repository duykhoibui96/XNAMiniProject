using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.GameComponent
{
    class Vision
    {
        public enum Direction
        {
            None = -1,
            Bottom = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            BottomLeft = 5,
            TopRight = 6,
            BottomRight = 7
        }

        public Direction Dir { get; private set; }
        private Rectangle[] visionRects;

        public Vision(Direction direction)
        {
            this.Dir = direction;
        }

        public Vision(Direction direction, Rectangle entityRect)
            : this(direction)
        {
            visionRects = new Rectangle[]
            {
                Rectangle.Empty,
                Rectangle.Empty,
                Rectangle.Empty,
                Rectangle.Empty
            };
        }

        public void Update(Rectangle currentRect, TerrainManager terrainManager)
        {
            var unit = Configuration.Unit;
            var rect = currentRect;
            int offsetX = 0, offsetY = 0;
            switch (Dir)
            {
                case Direction.Bottom:
                    offsetY = unit;
                    break;
                case Direction.Left:
                    offsetX = -unit;
                    break;
                case Direction.Right:
                    offsetX = unit;
                    break;
                case Direction.Top:
                    offsetY = -unit;
                    break;
                case Direction.TopLeft:
                    offsetX = -unit;
                    offsetY = -unit;
                    break;
                case Direction.BottomLeft:
                    offsetX = -unit;
                    offsetY = unit;
                    break;
                case Direction.TopRight:
                    offsetX = unit;
                    offsetY = -unit;
                    break;
                case Direction.BottomRight:
                    offsetX = unit;
                    offsetY = unit;
                    break;
            }

            for (int i = 0; i < visionRects.Length; i++)
            {
                visionRects[i] = Rectangle.Empty;
                rect.Offset(offsetX, offsetY);
                if (terrainManager.isValidPosition(rect.Location))
                    visionRects[i] = rect;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var vision in visionRects.TakeWhile(visionRect => !visionRect.Equals(Rectangle.Empty)))
            {
                spriteBatch.Draw(ResManager.Instance.Vision, vision, Color.White);
            }
        }

        public bool Contains(Rectangle rect)
        {
            return visionRects.TakeWhile(visionRect => !visionRect.Equals(Rectangle.Empty)).Any(visionRect => visionRect.Equals(rect));
        }
    }
}
