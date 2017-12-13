using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.Entity
{
    class Effect : _2DEntity
    {
        private static int Delay = 50;

        public enum EffectType
        {
            Collision = 0,
            Angry = 1
        }

        private EffectType type;
        private _2DModel model;
        private int elapsedTime = 0;
        private int currentMovementIndex;
        private int numbersOfAnimation;

        public Effect(EffectType type)
        {
            this.type = type;
            Texture2D texture = null;
            int numbersOfAnimationType = -1;
            switch (type)
            {
                case EffectType.Collision:
                    texture = ResManager.Instance.Collision;
                    numbersOfAnimation = 4;
                    numbersOfAnimationType = 4;
                    break;
                case EffectType.Angry:
                    texture = ResManager.Instance.Flaming;
                    numbersOfAnimation = 5;
                    numbersOfAnimationType = 3;
                    break;
            }

            var sourceRect = new Rectangle(0, 0, texture.Width / numbersOfAnimation, texture.Height / numbersOfAnimationType);
            model = new _2DModel(texture, Rectangle.Empty, Color.White, sourceRect);
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= Delay)
            {
                elapsedTime = 0;
                currentMovementIndex++;
                if (currentMovementIndex == 4) currentMovementIndex = 0;
                var sourceRect = model.SourceRect;
                sourceRect.X = sourceRect.Width*currentMovementIndex;
                model.SourceRect = sourceRect;
            }
        }

        public Rectangle Rect
        {
            set { model.Rect = value; }
        }

        public override _2DModel[] Models
        {
            get
            {
                return new _2DModel[]
                {
                    model
                };
            }
        }
    }
}
