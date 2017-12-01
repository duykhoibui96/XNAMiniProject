using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.GameComponent;

namespace MiniProject2D.Model
{
    class AnimationEntity : GameEntity
    {
        private Rectangle sourceRect;
        private SpriteBuilder spriteBuilder;
        private int currentAnimationType = 0;
        public int NumbersOfFrameType = 0;

        public bool AnimationMode
        {
            set
            {
                if (value)
                {
                    spriteBuilder.FrameType = currentAnimationType;
                }
                else
                {
                    spriteBuilder.FrameType = -1;
                }
            }
        }

        public int AnimationType
        {
            set
            {
                currentAnimationType = value;
            }
        }

        public AnimationEntity(Texture2D sprite, Rectangle rect, Color color, int numbersOfFrames, int defaultFrameIndex, int numbersOfFrameType = 4)
            : base(sprite, rect, color)
        {
            spriteBuilder = new SpriteBuilder(sprite.Width / numbersOfFrames, sprite.Height / numbersOfFrameType, numbersOfFrames, defaultFrameIndex);
            sourceRect = spriteBuilder.GetFrameRect();
        }

        public override void Update(GameTime gameTime)
        {
            spriteBuilder.Update(gameTime);
            sourceRect = spriteBuilder.GetFrameRect();
        }

        public override void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            if (IsVisible)
                spriteBatch.Draw(Sprite, Rect, sourceRect, isDisabled ? Color.Gray : CurrentColor);
        }
    }
}
