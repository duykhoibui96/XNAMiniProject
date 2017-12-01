using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;

namespace MiniProject2D.GameComponent
{
    class SpriteBuilder
    {

        private int frameWidth;
        private int frameHeight;
        private int numbersOfFrames;
        private int defaultFrameIndex;
        private int frameType;
        private int frameIndex;
        private int elapsedTime = 0;
        private bool inAnimationMode;


        public int FrameType
        {
            get
            {
                return frameType;
            }
            set
            {
                if (value == -1)
                {
                    frameIndex = defaultFrameIndex;
                    elapsedTime = 0;
                    inAnimationMode = false;
                    return;
                }

                inAnimationMode = true;
                if (value > 3)
                    switch (value)
                    {
                        case 4:
                        case 5:
                            frameType = 1;
                            break;
                        default:
                            frameType = 2;
                            break;
                    }
                else
                    frameType = value;
            }
        }

        public SpriteBuilder(int frameWidth, int frameHeight, int numbersOfFrames, int defaultFrameIndex)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.numbersOfFrames = numbersOfFrames;
            this.defaultFrameIndex = defaultFrameIndex;
            frameIndex = defaultFrameIndex;
        }

        public void Update(GameTime gameTime)
        {
            if (inAnimationMode)
            {
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedTime <= Configuration.CharacterSwitchFrameDelay) return;
                elapsedTime = 0;
                frameIndex++;
                if (frameIndex >= numbersOfFrames) frameIndex = 0;
            }
            else
            {
                elapsedTime = 0;
                frameIndex = defaultFrameIndex;
            }
        }

        public Rectangle GetFrameRect()
        {
            return new Rectangle(frameIndex * frameWidth, FrameType * frameHeight, frameWidth, frameHeight);
        }

    }
}
