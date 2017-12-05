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

        public Point StartPoint;

        private int numbersOfFreeSpace;


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

        public int NumbersOfFreeSpace
        {
            get { return numbersOfFreeSpace; }
            private set { numbersOfFreeSpace = value; }
        }

        private BoundaryPositionType entrancePositionType;
        private BoundaryPositionType exitPositionType;

        public TerrainManager()
        {
            var unit = Configuration.Unit;
            var mapWidth = Setting.Instance.MapWidth;
            var mapHeight = Setting.Instance.MapHeight;
            boundary = new BackgroundEntity(ResManager.Instance.Boundary, new Rectangle(unit, unit, unit * mapWidth, unit * mapHeight), Color.White);
            area = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(boundary.Rect.X * 2, boundary.Rect.Y * 2, boundary.Rect.Width - 2 * unit, boundary.Rect.Height - 2 * unit), Color.White);
            entrance = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(unit * 2, unit * 2, unit, unit), Color.White);
            exit = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(unit * 2, unit * 2, unit, unit), Color.White);

            numbersOfFreeSpace = (mapWidth - 2) * (mapHeight - 2);
        }

        public void Init(int startX, int startY, int numOfObstacles)
        {
            var unit = Configuration.Unit;

            StartPoint = new Point(startX, startY);

            boundary.Rect.Location = StartPoint;
            boundary.Rect.Width = unit * Setting.Instance.MapWidth;
            boundary.Rect.Height = unit * Setting.Instance.MapHeight;

            area.Rect.Location = StartPoint;
            area.Rect.Offset(unit, unit);
            area.Rect.Width = boundary.Rect.Width - 2 * unit;
            area.Rect.Height = boundary.Rect.Height - 2 * unit;

            RandomObstacles(numOfObstacles);
            RandomDoors();


        }

        public bool isValidPosition(Point newPosition)
        {
            return entrance.Rect.Location.Equals(newPosition) || exit.Rect.Location.Equals(newPosition) || (area.Rect.Contains(newPosition) && obstacles.All(obstacle => !obstacle.Rect.Location.Equals(newPosition)));
        }

        public void Draw(SpriteBatch spriteBatch, bool isDisabled = true)
        {
            boundary.Draw(spriteBatch, isDisabled);
            area.Draw(spriteBatch, isDisabled);
            entrance.Draw(spriteBatch, isDisabled);
            spriteBatch.Draw(doorArrow, entrance.Rect, isDisabled ? Color.Gray : Color.White);
            exit.Draw(spriteBatch, isDisabled);
            spriteBatch.Draw(doorArrow, exit.Rect, isDisabled ? Color.Gray : Color.White);
            foreach (var obj in obstacles)
            {
                obj.Draw(spriteBatch, isDisabled);
            }
        }


        private void RandomObstacles(int numOfObstacles)
        {
            var unit = Configuration.Unit;
            var rand = Configuration.Rand;
            var mapWidth = Setting.Instance.MapWidth;
            var mapHeight = Setting.Instance.MapHeight;
            obstacles = new BackgroundEntity[numOfObstacles];
            var posList = new List<Point>();
            for (int index = 0; index < obstacles.Length; index++)
            {
                var horizontalPosition = rand.Next(0, mapWidth - 2);
                var verticalPosition = rand.Next(0, mapHeight - 2);
                var pos = new Point(area.Rect.X + unit * horizontalPosition, area.Rect.Y + unit * verticalPosition);

                while (posList.Contains(pos))
                {
                    horizontalPosition = rand.Next(0, mapWidth - 2);
                    verticalPosition = rand.Next(0, mapHeight - 2);
                    pos.X = area.Rect.X + unit*horizontalPosition;
                    pos.Y = area.Rect.Y + unit * verticalPosition;
                }

                posList.Add(pos);
                numbersOfFreeSpace--;
                obstacles[index] = new BackgroundEntity(ResManager.Instance.Wall, new Rectangle(pos.X, pos.Y, unit, unit), Color.White);
            }
        }

        private void RandomDoors()
        {
            var rand = Configuration.Rand;
            var mapWidth = Setting.Instance.MapWidth;
            var mapHeight = Setting.Instance.MapHeight;
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
                case BoundaryPositionType.Left:
                    exitPositionType = BoundaryPositionType.Right;
                    doorArrow = ResManager.Instance.ArrowRight;
                    break;
                case BoundaryPositionType.Right:
                    exitPositionType = BoundaryPositionType.Left;
                    doorArrow = ResManager.Instance.ArrowLeft;
                    break;
            }

            var entrancePositionIndex = entrancePositionType > (BoundaryPositionType)1 ? rand.Next(1, mapHeight - 1) : rand.Next(1, mapWidth - 1);
            var exitPositionIndex = exitPositionType > (BoundaryPositionType)1 ? rand.Next(1, mapHeight - 1) : rand.Next(1, mapWidth - 1);

            entrance.Rect.Location = GetPosition(entrancePositionType, entrancePositionIndex);
            exit.Rect.Location = GetPosition(exitPositionType, exitPositionIndex);


        }

        private Point GetPosition(BoundaryPositionType positionType, int positionIndex)
        {
            var startX = StartPoint.X;
            var startY = StartPoint.Y;

            var mapWidth = Setting.Instance.MapWidth;
            var mapHeight = Setting.Instance.MapHeight;


            switch (positionType)
            {
                case BoundaryPositionType.Top: //TOP
                    return new Point(startX + Configuration.Unit * positionIndex, startY);
                case BoundaryPositionType.Bottom: //BOTTOM
                    return new Point(startX + Configuration.Unit * positionIndex, startY + Configuration.Unit * (mapHeight - 1));
                case BoundaryPositionType.Left: //LEFT
                    return new Point(startX, startY + Configuration.Unit * positionIndex);
                case BoundaryPositionType.Right: //RIGHT
                    return new Point(startX + Configuration.Unit * (mapWidth - 1), startY + Configuration.Unit * positionIndex);

            }

            return Point.Zero;
        }


    }
}
