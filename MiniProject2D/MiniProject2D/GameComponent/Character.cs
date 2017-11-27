using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.GameComponent
{
    class Character
    {

        public enum ObjectType
        {
            Player = 0,
            Mummy = 1,
            Scorpion = 2,
            Zombie = 3
        }

        private Vision.Direction currentMovementState;

        private bool isHover;
        public VisionManager VisionList;
        public AnimationEntity MovementEntity;
        public AnimationEntity StateEntity;
        public bool MaxPower;

        public ObjectType ObjType;

        public Vision.Direction CurrentMovementState
        {
            get { return currentMovementState; }
            set
            {
                currentMovementState = value;
                MovementEntity.AnimationType = (int)value;
            }
        }

        public Character(AnimationEntity movementEntity, ObjectType objType)
        {
            MovementEntity = movementEntity;
            ObjType = objType;
            currentMovementState = Vision.Direction.None;
            var stateRect = movementEntity.Rect;
            stateRect.Width += 20;
            stateRect.Height += 20;
            StateEntity = new AnimationEntity(ResManager.Instance.Flaming, stateRect, Color.White, 5, 0, 3)
            {
                AnimationMode = true
            };
            VisionList = new VisionManager(MovementEntity.Rect,objType);
        }

        public void Update(GameTime gameTime)
        {
            MovementEntity.Update(gameTime);
            VisionList.Update(MovementEntity.Rect);
            if (!MaxPower) return;
            StateEntity.Update(gameTime);
            var stateRect = MovementEntity.Rect;
            stateRect.Offset(-10, -10);
            StateEntity.Rect.Location = stateRect.Location;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MovementEntity.Draw(spriteBatch);
            if (MaxPower)
                StateEntity.Draw(spriteBatch);
        }

    }
}
