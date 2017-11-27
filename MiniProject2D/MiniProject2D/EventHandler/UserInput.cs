using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MiniProject2D.EventHandler
{
    class UserInput
    {
        public static UserInput Instance;

        private UserInput() { }

        static UserInput()
        {
            Instance = new UserInput();
        }

        private int mouseLeftButtonPressedX = 0;
        private int mouseLeftButtonPressedY = 0;
        private bool isLeftButtonPressed = false;
        public Keys PressedKey;
        public bool IsLeftClick;

        public void Update()
        {
            IsLeftClick = false;
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            var mouseState = Mouse.GetState();
            PressedKey = pressedKeys.Length > 0 ? pressedKeys[0] : Keys.None;
            if (!isLeftButtonPressed)
            {
                isLeftButtonPressed = mouseState.LeftButton == ButtonState.Pressed;
                if (isLeftButtonPressed)
                {
                    mouseLeftButtonPressedX = mouseState.X;
                    mouseLeftButtonPressedY = mouseState.Y;
                }
            }
            if (mouseState.LeftButton != ButtonState.Released || !isLeftButtonPressed) return;
            isLeftButtonPressed = false;
            if (mouseState.X == mouseLeftButtonPressedX && mouseState.Y == mouseLeftButtonPressedY)
            {
                IsLeftClick = true;
            }
        }


    }
}
