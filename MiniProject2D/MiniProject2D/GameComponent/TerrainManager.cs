using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.GameComponent
{
    class TerrainManager
    {
        public enum BoundaryPositionType
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3
        }

        private BackgroundEntity area;
        private BackgroundEntity entrance;
        private BackgroundEntity exit;
        private BackgroundEntity boundary;
        private BackgroundEntity[] obstacles;
        private Texture2D doorArrow;

        public Point EntrancePos
        {
            get { return entrance.Rect.Location; }
        }

        public Point ExitPos
        {
            get { return exit.Rect.Location; }
        }

        public Point[] ObstaclePos
        {
            get { return obstacles.Select(obstacle => obstacle.Rect.Location).ToArray(); }
        }

        public Rectangle Map
        {
            get { return boundary.Rect; }
        }

        public Rectangle InsideMap
        {
            get { return area.Rect; }
        }

        private BoundaryPositionType entrancePositionType;
        private BoundaryPositionType exitPositionType;

        public TerrainManager()
        {
            var unit = Configuration.Unit;
            boundary = new BackgroundEntity(ResManager.Instance.Boundary, new Rectangle(unit, unit, unit * 20, unit * 10), Color.White);
            area = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(boundary.Rect.X * 2, boundary.Rect.Y * 2, boundary.Rect.Width - 2 * unit, boundary.Rect.Height - 2 * unit), Color.White);
            entrance = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(unit * 2, unit * 2, unit, unit), Color.White);
            exit = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(unit * 2, unit * 2, unit, unit), Color.White);   

        }

        public void Init(int numOfObstacles)
        {
            var unit = Configuration.Unit;

            boundary.Rect.Width = unit * 20;
            boundary.Rect.Height = unit * 10;

            area.Rect.Width = 900;
            area.Rect.Height = 400;

            RandomObstacles(numOfObstacles);
            RandomDoors();


        }

        public bool isValidPosition(Point newPosition)
        {
            return entrance.Rect.Location.Equals(newPosition) || exit.Rect.Location.Equals(newPosition) || (area.Rect.Contains(newPosition) && obstacles.All(obstacle => !obstacle.Rect.Location.Equals(newPosition)));
        }

        public void Draw(SpriteBatch spriteBatch,bool isDisabled = true)
        {
            boundary.Draw(spriteBatch,isDisabled);
            area.Draw(spriteBatch,isDisabled);
            entrance.Draw(spriteBatch,isDisabled);
            spriteBatch.Draw(doorArrow, entrance.Rect, isDisabled ? Color.Gray : Color.White);
            exit.Draw(spriteBatch,isDisabled);
            spriteBatch.Draw(doorArrow, exit.Rect, isDisabled ? Color.Gray : Color.White);
            foreach (var obj in obstacles)
            {
                obj.Draw(spriteBatch,isDisabled);
            }
        }


        private void RandomObstacles(int numOfObstacles)
        {
            var unit = Configuration.Unit;
            var rand = Configuration.Rand;
            obstacles = new BackgroundEntity[numOfObstacles];
            for (int index = 0; index < obstacles.Length; index++)
            {
                var horizontalPosition = rand.Next(2, 20);
                var verticalPosition = rand.Next(2, 10);
                obstacles[index] = new BackgroundEntity(ResManager.Instance.Wall, new Rectangle(unit * horizontalPosition, unit * verticalPosition, unit, unit), Color.White);
            }
        }

        private void RandomDoors()
        {
            var rand = Configuration.Rand;
            //Entrance's position must be opposite to exit's

            entrancePositionType = (BoundaryPositionType)rand.Next(4);//Random from 0 -> 3
            // entrancePositionType = BoundaryPositionType.Bottom;
            switch (entrancePositionType)
            {
                case BoundaryPositionType.Top:
                    exitPositionType = BoundaryPositionType.Bottom;
                    doorArrow = ResManager.Instance.ArrowDown;
                    break;
                case BoundaryPositionType.Bottom:
                    exitPositionType = BoundaryPositionType.Top;
                    doorArrow = ResManager.Instance.ArrowUp;
                    break;
                case BoundaryPositionType.Left: //LEFT
                    exitPositionType = BoundaryPositionType.Right;
                    doorArrow = ResManager.Instance.ArrowRight;
                    break;
                case BoundaryPositionType.Right: //RIGHT
                    exitPositionType = BoundaryPositionType.Left;
                    doorArrow = ResManager.Instance.ArrowLeft;
                    break;
            }

            var entrancePositionIndex = entrancePositionType > (BoundaryPositionType)1 ? rand.Next(2, 10) : rand.Next(2, 20);
            var exitPositionIndex = exitPositionType > (BoundaryPositionType)1 ? rand.Next(2, 10) : rand.Next(2, 20);

            entrance.Rect.Location = GetPosition(entrancePositionType, entrancePositionIndex);
            exit.Rect.Location = GetPosition(exitPositionType, exitPositionIndex);


        }

        private Point GetPosition(BoundaryPositionType positionType, int positionIndex)
        {
            switch (positionType)
            {
                case BoundaryPositionType.Top: //TOP
                    return new Point(Configuration.Unit * positionIndex, Configuration.Unit);
                case BoundaryPositionType.Bottom: //BOTTOM
                    return new Point(Configuration.Unit * positionIndex, Configuration.Unit * 10);
                case BoundaryPositionType.Left: //LEFT
                    return new Point(Configuration.Unit, Configuration.Unit * positionIndex);
                case BoundaryPositionType.Right: //RIGHT
                    return new Point(Configuration.Unit * 20, Configuration.Unit * positionIndex);

            }

            return Point.Zero;
        }


    }
}
