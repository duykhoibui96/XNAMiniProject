using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.GameComponent;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class PlayingView : GameView
    {
        public enum State
        {
            Start = 0,
            Processing = 1,
            Pause = 2,
            Lose = 3,
            Win = 4,
        }

        private CharacterManager characterManager;
        private TerrainManager terrainManager;
        private BackgroundEntity background;
        private AnimationEntity explosion;
        private ClickableEntity config;
        private State state;
        private int endGameDelayTime;

        public override ViewMode Mode
        {
            get { return base.Mode; }
            set
            {
                base.Mode = value;
                if (value == ViewMode.CURRENT)
                    state = State.Processing;
                else if (value == ViewMode.DISABLED)
                    state = State.Pause;
            }
        }

        public PlayingView()
            : base()
        {
            Type = ViewType.PlayingView;
            SoundManager.Instance.PlayMusic(ResManager.Instance.GameMusic);
        }

        public override void Init(GraphicsDevice graphicsDevice)
        {
            var unit = Configuration.Unit;
            int numbersOfObstacles, numbersOfZombie, numbersOfScorpion, numbersOfMummy;
            GetComponentQuanlities(out numbersOfObstacles, out numbersOfMummy, out numbersOfScorpion,
                out numbersOfZombie);

            explosion = new AnimationEntity(ResManager.Instance.Collision, new Rectangle(0, 0, 100, 100), Color.White, 4, 0);
            config = new ClickableEntity(EventBoard.Event.PauseGame, ResManager.Instance.Config,
                ResManager.Instance.ConfigHover,
                new Rectangle(0, 0, unit * 2, unit * 2), Color.White);
            background = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            terrainManager = new TerrainManager();
            characterManager = new CharacterManager();

            var map = terrainManager.Map;

            state = State.Start;
            config.Rect.X = map.X + map.Width + unit;
            explosion.IsVisible = false;
            endGameDelayTime = 1000;

            terrainManager.Init(numbersOfObstacles);
            characterManager.Init(terrainManager, numbersOfMummy, numbersOfScorpion, numbersOfZombie);

        }

        private static void GetComponentQuanlities(out int numbersOfObstacles, out int numbersOfMummy, out int numbersOfScorpion, out int numbersOfZombie)
        {
            numbersOfObstacles = 20;
            numbersOfMummy = 2;
            numbersOfScorpion = 1;
            numbersOfZombie = 1;
        }

        public override void Update(GameTime gameTime)
        {
            if (mode != ViewMode.CURRENT) return;
            switch (state)
            {
                case State.Start:
                    state = State.Processing;
                    break;
                case State.Processing:
                    config.Update(gameTime);
                    characterManager.Update(gameTime);
                    if (characterManager.IsWon())
                        state = State.Win;
                    if (characterManager.CheckCollision())
                        state = State.Lose;
                    break;
                case State.Pause:
                    break;
                case State.Win:
                    EventBoard.Instance.AddEvent(EventBoard.Event.ShowResultsWhenWin);
                    state = State.Pause;
                    mode = ViewMode.DISABLED;
                    break;
                case State.Lose:
                    if (!explosion.IsVisible)
                    {
                        explosion.IsVisible = true;
                        explosion.AnimationMode = true;
                        explosion.Rect.Location = characterManager.CollisionPos;
                        explosion.Rect.Offset(-25, -25);
                        SoundManager.Instance.PlaySound(ResManager.Instance.Explosion);
                    }
                    explosion.Update(gameTime);
                    endGameDelayTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (endGameDelayTime <= 0)
                    {
                        EventBoard.Instance.AddEvent(EventBoard.Event.ShowResultsWhenLose);
                        state = State.Pause;
                        mode = ViewMode.DISABLED;
                    }
                    break;
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = mode == ViewMode.DISABLED;
            background.Draw(spriteBatch, isDisabled);
            terrainManager.Draw(spriteBatch, isDisabled);
            characterManager.Draw(spriteBatch, isDisabled);
            explosion.Draw(spriteBatch, isDisabled);
            config.Draw(spriteBatch, isDisabled);
        }

    }
}
