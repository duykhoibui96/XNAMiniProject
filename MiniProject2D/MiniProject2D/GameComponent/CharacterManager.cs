using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.GameComponent;

namespace MiniProject2D.GameComponent
{
    class CharacterManager
    {
        public Character Object;
        private Rectangle newRect;
        public int NumbersOfSteps = 0;

        public bool FinishMoving
        {
            get
            {
                return newRect.Equals(Object.MovementEntity.Rect);
            }
        }

        public CharacterManager(Character obj)
        {
            Object = obj;
            newRect = obj.MovementEntity.Rect;
        }

        public void ApplyNewCoords(Rectangle newRect,Character.MovementState movementState)
        {
            this.newRect = newRect;
            Object.CurrentMovementState = movementState;
            Object.MovementEntity.AnimationMode = true;
        }

        public void Move()
        {
            int offsetX = 0, offsetY = 0;
            switch (Object.CurrentMovementState)
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
            Object.MovementEntity.Rect.Offset(offsetX, offsetY);
        }

        public void StopMoving()
        {
            Object.MovementEntity.AnimationMode = false;
            Object.CurrentMovementState = Character.MovementState.Stand;
        }
    }
}
