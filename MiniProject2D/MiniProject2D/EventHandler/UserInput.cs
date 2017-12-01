using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MiniProject2D.EventHandler
{
    internal class UserInput
    {
        public static UserInput Instance;

        private UserInput()
        {
        }

        static UserInput()
        {
            Instance = new UserInput();
        }

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;
        private MouseState previousMouseState;
        private MouseState currentMouseState;

        public Keys PressedKey;
        public bool IsLeftClick;

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            IsLeftClick = previousMouseState.LeftButton.Equals(ButtonState.Pressed) &&
                          currentMouseState.LeftButton.Equals(ButtonState.Pressed);
            var pressedKeys = currentKeyboardState.GetPressedKeys();
            PressedKey = pressedKeys.Length > 0 ? pressedKeys[0] : Keys.None;

        }
    }
}
