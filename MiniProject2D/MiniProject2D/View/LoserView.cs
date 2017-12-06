using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class LoserView : GameView
    {
        private Vector2 notifyPos;
        private SpriteFont notifyFont;

        public LoserView()
            : base()
        {
            Type = ViewType.LoserView;
            SoundManager.Instance.PlaySound(ResManager.Instance.LoseSound);
        }

        public override void Init(Rectangle viewContainer)
        {
            base.Init(viewContainer);
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            notifyFont = ResManager.Instance.NotifyFont;
            notifyPos = new Vector2(viewContainer.X + viewContainer.Width / 2, viewContainer.Y + unit * 2);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            string output = "YOU LOSE!";
            // Find the center of the string
            var FontOrigin = notifyFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(notifyFont, output, notifyPos, Color.Blue,
                0, FontOrigin, 3.0f, SpriteEffects.None, 0.5f);
        }
    }
}
