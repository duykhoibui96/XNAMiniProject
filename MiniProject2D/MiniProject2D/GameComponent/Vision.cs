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

        public Rectangle GetNearestVision()
        {
            return visionRects[0];
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
            Update(entityRect);
            //Update(entityRect);
            //var unit = Configuration.Unit;
            //int x = entityRect.X, y = entityRect.Y;
            //switch (direction)
            //{
            //    case Direction.Bottom:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x,y+unit,unit,unit),
            //            new Rectangle(x,y+2*unit,unit,unit),
            //            new Rectangle(x,y+3*unit,unit,unit),
            //            new Rectangle(x,y+4*unit,unit,unit),
            //        };
            //        break;
            //    case Direction.Left:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x - unit,y,unit,unit),
            //            new Rectangle(x - 2*unit,y,unit,unit),
            //            new Rectangle(x - 3*unit,y,unit,unit),
            //            new Rectangle(x - 4*unit,y,unit,unit),
            //        };
            //        break;
            //    case Direction.Right:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x + unit,y,unit,unit),
            //            new Rectangle(x + 2*unit,y,unit,unit),
            //            new Rectangle(x + 3*unit,y,unit,unit),
            //            new Rectangle(x + 4*unit,y,unit,unit),
            //        };
            //        break;
            //    case Direction.Top:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x,y-unit,unit,unit),
            //            new Rectangle(x,y-2*unit,unit,unit),
            //            new Rectangle(x,y-3*unit,unit,unit),
            //            new Rectangle(x,y-4*unit,unit,unit),
            //        };
            //        break;
            //    case Direction.TopLeft:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x-unit,y-unit,unit,unit),
            //            new Rectangle(x-2*unit,y-2*unit,unit,unit),
            //            new Rectangle(x-3*unit,y-3*unit,unit,unit),
            //            new Rectangle(x-4*unit,y-4*unit,unit,unit),
            //        };
            //        break;
            //    case Direction.BottomLeft:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x-unit,y+unit,unit,unit),
            //            new Rectangle(x-2*unit,y+2*unit,unit,unit),
            //            new Rectangle(x-3*unit,y+3*unit,unit,unit),
            //            new Rectangle(x-4*unit,y+4*unit,unit,unit),
            //        };
            //        break;
            //    case Direction.TopRight:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x+unit,y-unit,unit,unit),
            //            new Rectangle(x+2*unit,y-2*unit,unit,unit),
            //            new Rectangle(x+3*unit,y-3*unit,unit,unit),
            //            new Rectangle(x+4*unit,y-4*unit,unit,unit),
            //        };
            //        break;
            //    case Direction.BottomRight:
            //        visionRects = new Rectangle[]
            //        {
            //            new Rectangle(x+unit,y+unit,unit,unit),
            //            new Rectangle(x+2*unit,y+2*unit,unit,unit),
            //            new Rectangle(x+3*unit,y+3*unit,unit,unit),
            //            new Rectangle(x+4*unit,y+4*unit,unit,unit),
            //        };
            //        break;
            //}
        }

        public void Update(Rectangle newRect)
        {
            var unit = Configuration.Unit;
            var rect = newRect;
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
                rect.Offset(offsetX, offsetY);
                visionRects[i] = rect;
            }
        }

        public void Filter(Vision currentVision, Rectangle area, Rectangle entrance, Rectangle exit, BackgroundEntity[] obstacles)
        {
            if (!Dir.Equals(currentVision.Dir)) return;
            visionRects = currentVision.visionRects;
            for (int i = 0; i < 4; i++)
            {
                if ((!area.Contains(visionRects[i]) && !(entrance.Equals(visionRects[i]) || exit.Equals(visionRects[i]))) || obstacles.Any(obstacle => obstacle.Rect.Equals(visionRects[i])))
                    visionRects[i] = Rectangle.Empty;
            }

        }

        public bool CanKeepMoving()
        {
            return !visionRects[0].Equals(Rectangle.Empty);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var vision in visionRects.TakeWhile(visionRect => !visionRect.Equals(Rectangle.Empty)))
            {
                spriteBatch.Draw(ResManager.Instance.Vision, vision, Color.White);
            }
        }

        public bool Discover(Rectangle rect)
        {
            return visionRects.TakeWhile(visionRect => !visionRect.Equals(Rectangle.Empty)).Any(vision => vision.Equals(rect));
        }
    }
}
