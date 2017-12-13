using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.Model;

namespace MiniProject2D.Entity
{
    abstract class MazeObject : _2DEntity
    {
        public static Point MazeStartPoint;

        public enum Type
        {
            Empty = -1,
            Obstacle = 0,
            Player = 1,
            Mummy = 2,
            Scorpion = 3,
            Zombie = 4
        }

        protected Type type;
        protected Point logicPos;

        public Type ObjectType
        {
            get { return type; }
        }

        public virtual Point LogicPos
        {
            get { return logicPos; }
            set { logicPos = value; }
        }

        public static Point MapToRealPosition(int x, int y)
        {
            var unit = Global.Instance.Unit;
            var pos = MazeStartPoint;

            pos.X += unit * x;
            pos.Y += unit * y;

            return pos;

        }

        public static Point MapToRealPosition(Point pos)
        {
            return MapToRealPosition(pos.X, pos.Y);
        }

        protected MazeObject()
        {

        }

        protected MazeObject(Point logicPos)
        {
            this.logicPos = logicPos;
        }

    }
}
