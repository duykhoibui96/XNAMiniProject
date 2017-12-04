using Microsoft.Xna.Framework.Input;

namespace MiniProject2D.Input
{
    internal class KeyboardEvent
    {
        public static KeyboardEvent Instance;

        private KeyboardEvent()
        {
        }

        static KeyboardEvent()
        {
            Instance = new KeyboardEvent();
        }

        private KeyboardState previousState;
        private KeyboardState currentState;

        public Keys PressedKey;

        public void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            var pressedKeys = currentState.GetPressedKeys();
            PressedKey = pressedKeys.Length > 0 ? pressedKeys[0] : Keys.None;
        }
    }
}
