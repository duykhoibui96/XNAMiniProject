using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Model;

namespace MiniProject2D.GameComponent
{
    class Character
    {
        public enum MovementState
        {
            Stand = -1,
            MoveDown = 0,
            MoveLeft = 1,
            MoveRight = 2,
            MoveUp = 3,
            MoveLeftUp = 4,
            MoveLeftBottom = 5,
            MoveRightUp = 6,
            MoveRightBottom = 7
        }

        public enum ObjectType
        {
            Player = 0,
            Mummy = 1,
            Scorpion = 2,
            Zombie = 3
        }

        private MovementState currentMovementState;

        private bool isHover;
        public Rectangle[] Visions;
        public AnimationEntity MovementEntity;
        public AnimationEntity StateEntity;

        public ObjectType ObjType;

        public MovementState CurrentMovementState
        {
            get { return currentMovementState; }
            set
            {
                currentMovementState = value;
                MovementEntity.AnimationType = (int)value;
            }
        }

        public Character(AnimationEntity momentEntity, ObjectType objType)
        {
            MovementEntity = momentEntity;
            ObjType = objType;
            currentMovementState = MovementState.Stand;
            switch (objType)
            {
                case ObjectType.Mummy:
                    Visions = new Rectangle[]
                    {
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit,
                        Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit * 2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit * 3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit * 4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit * 2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit * 3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit * 4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit * 2, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit * 3, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit * 4, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit * 2, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit * 3, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit * 4, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                    };

                    break;
                case ObjectType.Scorpion:
                    Visions = new Rectangle[]
                    {
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit,
                            MovementEntity.Rect.Y - Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*2,
                            MovementEntity.Rect.Y - Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*3,
                            MovementEntity.Rect.Y - Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*4,
                            MovementEntity.Rect.Y - Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit,
                            MovementEntity.Rect.Y + Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*2,
                            MovementEntity.Rect.Y + Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*3,
                            MovementEntity.Rect.Y + Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*4,
                            MovementEntity.Rect.Y + Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit,
                            MovementEntity.Rect.Y + Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*2,
                            MovementEntity.Rect.Y + Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*3,
                            MovementEntity.Rect.Y + Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*4,
                            MovementEntity.Rect.Y + Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit,
                            MovementEntity.Rect.Y - Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*2,
                            MovementEntity.Rect.Y - Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*3,
                            MovementEntity.Rect.Y - Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*4,
                            MovementEntity.Rect.Y - Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                    };

                    break;
                case ObjectType.Zombie:

                    Visions = new Rectangle[]
                    {
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit,
                        Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit * 2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit * 3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y - Configuration.Unit * 4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit * 2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit * 3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X, MovementEntity.Rect.Y + Configuration.Unit * 4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit * 2, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit * 3, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit * 4, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit * 2, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit * 3, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit * 4, MovementEntity.Rect.Y,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit,
                            MovementEntity.Rect.Y - Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*2,
                            MovementEntity.Rect.Y - Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*3,
                            MovementEntity.Rect.Y - Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*4,
                            MovementEntity.Rect.Y - Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit,
                            MovementEntity.Rect.Y + Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*2,
                            MovementEntity.Rect.Y + Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*3,
                            MovementEntity.Rect.Y + Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*4,
                            MovementEntity.Rect.Y + Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit,
                            MovementEntity.Rect.Y + Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*2,
                            MovementEntity.Rect.Y + Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*3,
                            MovementEntity.Rect.Y + Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X - Configuration.Unit*4,
                            MovementEntity.Rect.Y + Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit,
                            MovementEntity.Rect.Y - Configuration.Unit,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*2,
                            MovementEntity.Rect.Y - Configuration.Unit*2,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*3,
                            MovementEntity.Rect.Y - Configuration.Unit*3,
                            Configuration.Unit, Configuration.Unit),
                        new Rectangle(MovementEntity.Rect.X + Configuration.Unit*4,
                            MovementEntity.Rect.Y - Configuration.Unit*4,
                            Configuration.Unit, Configuration.Unit),

                    };
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            MovementEntity.Update(gameTime);
            if (Visions == null) return;
            for (int index = 0; index < Visions.Length; index++)
            {
                int offsetX = 0, offsetY = 0;
                switch (currentMovementState)
                {
                    case Character.MovementState.MoveDown:
                        offsetY = Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveLeft:
                        offsetX = -Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveRight:
                        offsetX = Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveUp:
                        offsetY = -Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveLeftUp:
                        offsetY = -Configuration.Velocity;
                        offsetX = -Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveLeftBottom:
                        offsetY = Configuration.Velocity;
                        offsetX = -Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveRightUp:
                        offsetY = -Configuration.Velocity;
                        offsetX = Configuration.Velocity;
                        break;
                    case Character.MovementState.MoveRightBottom:
                        offsetY = Configuration.Velocity;
                        offsetX = Configuration.Velocity;
                        break;
                }
                Visions[index].Offset(offsetX, offsetY);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MovementEntity.Draw(spriteBatch);
        }

    }
}
