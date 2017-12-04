using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        private MouseState previousState;
        private MouseState currentState;

        public Point MousePosition;
        public bool IsLeftClick;

        public void Update()
        {
            previousState = currentState;
            currentState = Mouse.GetState();
            IsLeftClick = previousState.LeftButton.Equals(ButtonState.Pressed) &&
                          currentState.LeftButton.Equals(ButtonState.Released);
            MousePosition.X = currentState.X;
            MousePosition.Y = currentState.Y;
        }
    }
}
