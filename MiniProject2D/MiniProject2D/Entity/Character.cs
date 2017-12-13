using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Resources;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.Entity
{
    class Character : MazeObject
    {

        private const int Delay = 50;

        public static Color PlayerColor = Color.White;
        public static Color MummyColor = Color.White;
        public static Color ScorpionColor = Color.White;
        public static Color ZombieColor = Color.White;


        public override _2DModel[] Models
        {
            get
            {
                var modelList = new List<_2DModel>();
                var frameWidth = model.Texture.Width / numbersOfMovement;
                var frameHeight = model.Texture.Height / 4;
                model.SourceRect = new Rectangle(currentMovementIndex * frameWidth, movementType * frameHeight, frameWidth, frameHeight);
                switch (type)
                {
                    case Type.Player:
                        model.CurrentColor = PlayerColor;
                        break;
                    case Type.Mummy:
                        model.CurrentColor = MummyColor;
                        break;
                    case Type.Scorpion:
                        model.CurrentColor = ScorpionColor;
                        break;
                    case Type.Zombie:
                        model.CurrentColor = ZombieColor;
                        break;
                }

                modelList.Add(model);
                if (isAngry)
                {
                    var rect = model.Rect;
                    var unit = Global.Instance.Unit;
                    rect.Offset(0,-unit);
                    discoveryEffect.Rect = rect;
                    modelList.Add(discoveryEffect);
                }

                return modelList.ToArray();

            }
        }

        public override Point LogicPos
        {
            get { return base.LogicPos; }
            set
            {
                base.LogicPos = value;
                currentPosition = MapToRealPosition(value.X, value.Y);
            }
        }



        public bool NeedMoving
        {
            get { return !model.Rect.Location.Equals(currentPosition); }
        }

        public int PreferredDirection
        {
            get { return preferredDirection; }
            set { preferredDirection = value; }
        }

        public bool IsAngry
        {
            get { return isAngry; }
            set
            {
                var angryBefore = isAngry;
                isAngry = value;

                if (isAngry && !angryBefore)
                {
                    SoundManager.Instance.PlaySound(ResManager.Instance.MonsterEncounter);
                }
            }
        }

        public int NumOfSteps
        {
            get { return numOfSteps; }
            set
            {
                if (numOfSteps == 0)
                    numOfSteps = value;
            }
        }

        private int defaultMovementIndex;
        private int numbersOfMovement;
        private int currentMovementIndex;
        private int elapsedTime;
        private int movementType;
        private int preferredDirection;
        private int numOfSteps;
        private bool isAngry;

        private _2DModel model;
        private Point currentPosition;
        private _2DModel discoveryEffect;



        public Character(Type type, int x, int y)
        {
            this.type = type;
            discoveryEffect = new _2DModel(ResManager.Instance.DiscoveryEffect, Rectangle.Empty, Color.White);
            logicPos = new Point(x, y);
            var res = ResManager.Instance;
            var unit = Global.Instance.Unit;
            var realPosition = MapToRealPosition(x, y);
            Texture2D texture;
            switch (type)
            {
                case Type.Player:
                    texture = res.Player;
                    numbersOfMovement = 4;
                    defaultMovementIndex = 2;
                    break;
                case Type.Mummy:
                    texture = res.Mummy;
                    numbersOfMovement = 3;
                    defaultMovementIndex = 1;
                    break;
                case Type.Scorpion:
                    texture = res.Scorpion;
                    numbersOfMovement = 4;
                    defaultMovementIndex = 2;
                    break;
                default:
                    texture = res.Zombie;
                    numbersOfMovement = 6;
                    defaultMovementIndex = 0;
                    break;
            }

            model = new _2DModel(texture, new Rectangle(realPosition.X, realPosition.Y, unit, unit), Color.White);
            movementType = 0;
            currentMovementIndex = defaultMovementIndex;
        }

        public void Move(GameTime gameTime)
        {
            var offsetX = currentPosition.X - model.Rect.Location.X;
            var offsetY = currentPosition.Y - model.Rect.Location.Y;

            if (offsetY > 0)
            {
                offsetY = 2;
                movementType = 0;
            }
            else if (offsetY < 0)
            {
                offsetY = -2;
                movementType = 3;
            }

            if (offsetX > 0)
            {
                offsetX = 2;
                movementType = 2;
            }
            else if (offsetX < 0)
            {
                offsetX = -2;
                movementType = 1;
            }


            model.Offset(offsetX, offsetY);

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= Delay)
            {
                elapsedTime = 0;
                currentMovementIndex++;
                if (currentMovementIndex == numbersOfMovement) currentMovementIndex = 0;
            }

        }

        public void Stop()
        {
            elapsedTime = 0;
            numOfSteps--;
            currentMovementIndex = defaultMovementIndex;
        }

    }
}
