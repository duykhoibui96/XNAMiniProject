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

        private bool isHover;
        public VisionManager VisionList;
        public AnimationEntity MovementEntity;
        public AnimationEntity StateEntity;
        public Vision.Direction PreferableDirection = Vision.Direction.None;
        public Point offsetPoint;
        public bool MaxPower;
        public int NumOfSteps = 0;

        public ObjectType ObjType;

        public Character(AnimationEntity movementEntity, ObjectType objType)
        {
            MovementEntity = movementEntity;
            offsetPoint = new Point(0, 0);
            ObjType = objType;
            var stateRect = movementEntity.Rect;
            stateRect.Width += 20;
            stateRect.Height += 20;
            StateEntity = new AnimationEntity(ResManager.Instance.Flaming, stateRect, Color.White, 5, 0, 3)
            {
                AnimationMode = true
            };
            VisionList = new VisionManager(MovementEntity.Rect, objType);
        }

        public void Update(GameTime gameTime, TerrainManager terrainManager)
        {
            var mouseState = Mouse.GetState();
            MovementEntity.Rect.Offset(offsetPoint);
            MovementEntity.Update(gameTime);
            VisionList.Update(MovementEntity.Rect, terrainManager);
            isHover = MovementEntity.Rect.Contains(mouseState.X, mouseState.Y);
            if (MaxPower)
            {
                StateEntity.Update(gameTime);
                var stateRect = MovementEntity.Rect;
                stateRect.Offset(-10, -10);
                StateEntity.Rect.Location = stateRect.Location;
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            MovementEntity.Draw(spriteBatch, isDisabled);
            if (isHover)
                VisionList.Draw(spriteBatch, isDisabled);
            if (MaxPower)
                StateEntity.Draw(spriteBatch, isDisabled);
        }

        public void StartMoving(Vision.Direction movementDirection)
        {
            var velocity = Configuration.Velocity;
            switch (movementDirection)
            {
                case Vision.Direction.Bottom:
                    offsetPoint.Y = velocity;
                    break;
                case Vision.Direction.Left:
                    offsetPoint.X = -velocity;
                    break;
                case Vision.Direction.Right:
                    offsetPoint.X = velocity;
                    break;
                case Vision.Direction.Top:
                    offsetPoint.Y = -velocity;
                    break;
                case Vision.Direction.TopLeft:
                    offsetPoint.X = -velocity;
                    offsetPoint.Y = -velocity;
                    break;
                case Vision.Direction.BottomLeft:
                    offsetPoint.X = -velocity;
                    offsetPoint.Y = velocity;
                    break;
                case Vision.Direction.TopRight:
                    offsetPoint.X = velocity;
                    offsetPoint.Y = -velocity;
                    break;
                case Vision.Direction.BottomRight:
                    offsetPoint.X = velocity;
                    offsetPoint.Y = velocity;
                    break;
            }

            MovementEntity.AnimationType = (int)movementDirection;
            MovementEntity.AnimationMode = true;

            if (NumOfSteps == 0)
            {
                NumOfSteps = PreferableDirection.Equals(Vision.Direction.None) ? 1 : 2;
            }
        }

        public void Stop()
        {
            offsetPoint.X = offsetPoint.Y = 0;
            MovementEntity.AnimationMode = false;
        }

        public Vision.Direction Find(Character player)
        {
            foreach (var vision in VisionList.Visions)
            {
                if (vision != null && vision.Contains(player.MovementEntity.Rect))
                {
                    if (PreferableDirection.Equals(Vision.Direction.None))
                    {
                        SoundManager.Instance.PlaySoundWhenEncounterMonster();
                    }
                    return vision.Dir;
                }
                    
            }

            return Vision.Direction.None;
        }
    }
}
