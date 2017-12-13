using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.Entity
{
    class Obstacle : MazeObject
    {
        private _2DModel model;

        public Obstacle(int x, int y)
        {
            var unit = Global.Instance.Unit;
            var realPosition = MapToRealPosition(x, y);
            logicPos = new Point(x,y);
            type = Type.Obstacle;
            model = new _2DModel(ResManager.Instance.Wall, new Rectangle(realPosition.X, realPosition.Y, unit, unit), Color.White);
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
