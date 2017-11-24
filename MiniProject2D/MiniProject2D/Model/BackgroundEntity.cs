using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.Model
{
    class BackgroundEntity: GameEntity
    {
        public BackgroundEntity(Texture2D sprite, Rectangle rect, Color color) : base(sprite, rect, color)
        {
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
