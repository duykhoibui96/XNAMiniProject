using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.GameComponent;
using MiniProject2D.Model;

namespace MiniProject2D.GameComponent
{
    class CharacterManager
    {
        private static Random random;
        public Character Object;
        private Rectangle newRect;
        private Vision.Direction playerDiscoveryDirection = Vision.Direction.None;
        private VisionManager visionList;
        public int NumbersOfSteps = 0;
        public bool IsHover;

        static CharacterManager()
        {
            random = new Random();
        }

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
            visionList = obj.VisionList;
        }

        public void ApplyNewCoords(Rectangle newRect, Vision.Direction movementState)
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
                case Vision.Direction.Bottom:
                    offsetY = Configuration.Velocity;
                    break;
                case Vision.Direction.Left:
                    offsetX = -Configuration.Velocity;
                    break;
                case Vision.Direction.Right:
                    offsetX = Configuration.Velocity;
                    break;
                case Vision.Direction.Top:
                    offsetY = -Configuration.Velocity;
                    break;
                case Vision.Direction.TopLeft:
                    offsetY = -Configuration.Velocity;
                    offsetX = -Configuration.Velocity;
                    break;
                case Vision.Direction.BottomLeft:
                    offsetY = Configuration.Velocity;
                    offsetX = -Configuration.Velocity;
                    break;
                case Vision.Direction.TopRight:
                    offsetY = -Configuration.Velocity;
                    offsetX = Configuration.Velocity;
                    break;
                case Vision.Direction.BottomRight:
                    offsetY = Configuration.Velocity;
                    offsetX = Configuration.Velocity;
                    break;
            }
            Object.MovementEntity.Rect.Offset(offsetX, offsetY);
        }

        public void Update(GameTime gameTime, Rectangle area, Rectangle entrance, Rectangle exit, BackgroundEntity[] obstacles)
        {
            Object.Update(gameTime);
            visionList.Filter(Object.VisionList, area, entrance, exit, obstacles);
            var mouseState = Mouse.GetState();
            IsHover = Object.MovementEntity.Rect.Contains(mouseState.X, mouseState.Y);
            Object.MaxPower = !playerDiscoveryDirection.Equals(Vision.Direction.None);
        }

        public bool ApplyDirection(Vision.Direction direction)
        {
            var vision = visionList.IsValidDirection(direction);
            if (vision == null) return false;
            newRect = vision.GetNearestVision();
            Object.CurrentMovementState = vision.Dir;
            Object.MovementEntity.AnimationMode = true;
            if (NumbersOfSteps == 0)
            {
                if (Object.MaxPower)
                    NumbersOfSteps = 2;
                else
                    NumbersOfSteps = 1;
            }
            return true;
        }

        public void StopMoving()
        {
            Object.MovementEntity.AnimationMode = false;
            Object.CurrentMovementState = Vision.Direction.None;
        }

        public void DrawVisions(SpriteBatch spriteBatch)
        {
            foreach (var vision in visionList.Visions.Where(vision => vision != null))
            {
                vision.Draw(spriteBatch);
            }
        }

        public void Discover(CharacterManager character)
        {
            var discoveryDirection =
                visionList.Visions.FirstOrDefault(vision => vision != null && vision.Discover(character.Object.MovementEntity.Rect));
            playerDiscoveryDirection = discoveryDirection != null ? discoveryDirection.Dir : Vision.Direction.None;
        }

        public Vision.Direction GenerateDirection()
        {
            if (!playerDiscoveryDirection.Equals(Vision.Direction.None)) return playerDiscoveryDirection;
            switch (Object.ObjType)
            {
                case Character.ObjectType.Mummy:
                    return (Vision.Direction)random.Next(4);
                case Character.ObjectType.Scorpion:
                    return (Vision.Direction)random.Next(4, 8);
                case Character.ObjectType.Zombie:
                    return (Vision.Direction)random.Next(8);
            }

            return Vision.Direction.None;
        }
    }
}
