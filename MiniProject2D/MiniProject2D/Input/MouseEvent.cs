using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.Input
{
    class MouseEvent
    {
        public static MouseEvent Instance;

        private MouseEvent()
        {
            MousePosition = Point.Zero;
        }

        static MouseEvent()
        {
            Instance = new MouseEvent();
        }

        private _2DModel model;

        private MouseState previousState;
        private MouseState currentState;

        public Point PreviousMousePosition;
        public Point MousePosition;
        public bool IsLeftClick;
        public bool IsHoldLeftMouse;

        public _2DModel CursorModel
        {
            get { return model; }
        }

        public void Update()
        {
            var unit = Global.Instance.Unit;
            previousState = currentState;
            PreviousMousePosition = MousePosition;
            currentState = Mouse.GetState();
            IsLeftClick = previousState.LeftButton.Equals(ButtonState.Pressed) &&
                          currentState.LeftButton.Equals(ButtonState.Released);
            IsHoldLeftMouse = previousState.LeftButton.Equals(ButtonState.Pressed) &&
                          currentState.LeftButton.Equals(ButtonState.Pressed);
            MousePosition.X = currentState.X;
            MousePosition.Y = currentState.Y;

            CursorModel.Rect = new Rectangle(MousePosition.X, MousePosition.Y, unit, unit);
        }

        public void InitMouseSprite()
        {
            model = new _2DModel(ResManager.Instance.Cursor, Rectangle.Empty, Color.White);
        }

        
    }
}
