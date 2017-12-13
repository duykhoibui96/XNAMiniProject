using Microsoft.Xna.Framework;

namespace MiniProject2D.UI
{
    abstract class GameUI
    {
        public enum ViewMode
        {
            CURRENT = 0,
            DISABLED = 1,
            INVISIBLE = 2
        }

        public enum UIType
        {
            Menu = 0,
            Setting = 1,
            Main = 2
        }

        protected ViewMode mode = ViewMode.CURRENT;
        protected UIType type = UIType.Menu;
        protected Rectangle viewContainer;

        public virtual ViewMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public UIType Type
        {
            get { return type; }
            set { type = value; }
        }

        public virtual void Update(GameTime gameTime)
        {
            HandleEvent();
        }

        public abstract void Render();
        protected abstract void HandleEvent();

    }
}
