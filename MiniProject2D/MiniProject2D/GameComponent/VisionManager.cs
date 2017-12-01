using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public void Update(Rectangle currentRect, TerrainManager terrainManager)
        {
            foreach (var vision in Visions.Where(vision => vision != null))
            {
                vision.Update(currentRect, terrainManager);
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            foreach (var vision in Visions.Where(vision => vision != null))
            {
                vision.Draw(spriteBatch);
            }
        }

    }
}
