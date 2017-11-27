using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Model;

namespace MiniProject2D.GameComponent
{
    class VisionManager
    {
        public Vision[] Visions;

        public VisionManager(Rectangle entityRect, Character.ObjectType entityType)
        {
            switch (entityType)
            {
                case Character.ObjectType.Mummy:
                case Character.ObjectType.Player:
                    Visions = new Vision[]
                    {
                        new Vision(Vision.Direction.Bottom, entityRect),
                        new Vision(Vision.Direction.Left, entityRect),
                        new Vision(Vision.Direction.Right, entityRect),
                        new Vision(Vision.Direction.Top, entityRect),

                        null,
                        null,
                        null,
                        null
                    };
                    break;
                case Character.ObjectType.Scorpion:
                    Visions = new Vision[]
                    {
                        null,
                        null,
                        null,
                        null,

                        new Vision(Vision.Direction.TopLeft, entityRect),
                        new Vision(Vision.Direction.BottomLeft, entityRect),
                        new Vision(Vision.Direction.TopRight, entityRect),
                        new Vision(Vision.Direction.BottomRight, entityRect)
                    };
                    break;
                case Character.ObjectType.Zombie:
                    Visions = new Vision[]
                    {
                        new Vision(Vision.Direction.Bottom, entityRect),
                        new Vision(Vision.Direction.Left, entityRect),
                        new Vision(Vision.Direction.Right, entityRect),
                        new Vision(Vision.Direction.Top, entityRect),

                        new Vision(Vision.Direction.TopLeft, entityRect),
                        new Vision(Vision.Direction.BottomLeft, entityRect),
                        new Vision(Vision.Direction.TopRight, entityRect),
                        new Vision(Vision.Direction.BottomRight, entityRect)
                    };
                    break;
            }
        }

        public void Filter(VisionManager currentVisionManager, Rectangle area, Rectangle entrance, Rectangle exit, BackgroundEntity[] obstacles)
        {
            var currentVisions = currentVisionManager.Visions;
            for (int i = 0; i < currentVisions.Length; i++)
            {
                if (currentVisions[i] != null)
                    Visions[i].Filter(currentVisions[i], area, entrance, exit, obstacles);
            }
        }

        public void Update(Rectangle newRect)
        {
            foreach (var vision in Visions.Where(vision => vision != null))
            {
                vision.Update(newRect);
            }
        }

        public Vision IsValidDirection(Vision.Direction direction)
        {
            int directionIndex = (int)direction;
            var valid = Visions[directionIndex] != null && Visions[directionIndex].CanKeepMoving();
            return valid ? Visions[directionIndex] : null;
        }
    }
}
