using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.GameComponent
{
    class GameMatch : GameView
    {
        public enum State
        {
            Start = 0,
            Processing = 1,
            Pause = 2,
            Lose = 3,//Thua -> bị quái vật giết
            Win = 4,//Thắng -> ra được mê cung
            End = 5
        }

        private CharacterManager characterManager;
        private TerrainManager terrainManager;
        private BackgroundEntity background;
        private AnimationEntity explosion;
        private ClickableEntity config;
        private State state;
        private int endGameDelayTime;

        public State GameState
        {
            get { return state; }
            set { state = value; }
        }

        public GameMatch(Game game)
            : base(ViewType.Match)
        {
            var unit = Configuration.Unit;
            explosion = new AnimationEntity(ResManager.Instance.Collision, new Rectangle(0, 0, 100, 100), Color.White, 4, 0);
            config = new ClickableEntity(EventBoard.Event.PauseGame, ResManager.Instance.Config,
                ResManager.Instance.ConfigHover,
                new Rectangle(0, 0, unit * 2, unit * 2), Color.White);
            background = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
            terrainManager = new TerrainManager();
            characterManager = new CharacterManager();
        }

        public void Init()
        {
            var unit = Configuration.Unit;
            var numbersOfObstacles = 20;
            var numbersOfZombie = 1;
            var numbersOfScorpion = 1;
            var numbersOfMummy = 1;

            var map = terrainManager.Map;

            GameState = State.Start;
            config.Rect.X = map.X + map.Width + unit;
            explosion.IsVisible = false;
            endGameDelayTime = 1000;

            terrainManager.Init(numbersOfObstacles);
            characterManager.Init(terrainManager, numbersOfMummy, numbersOfScorpion, numbersOfZombie);
            SetEnabled(true);
        }

        public override void Update(GameTime gameTime)
        {

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
                    EventBoard.Instance.Ev = EventBoard.Event.ShowResultsWhenWin;
                    state = State.End;
                    break;
                case State.Lose:
                    if (!explosion.IsVisible)
                    {
                        explosion.IsVisible = true;
                        explosion.AnimationMode = true;
                        explosion.Rect.Location = characterManager.CollisionPos;
                        explosion.Rect.Offset(-25, -25);
                    }
                    explosion.Update(gameTime);
                    endGameDelayTime -= (int) gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (endGameDelayTime <= 0)
                    {
                        EventBoard.Instance.Ev = EventBoard.Event.ShowResultsWhenLose;
                        state = State.End;
                    }
                    break;
                case State.End:
                    break;
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            var isDisabled = state.Equals(State.Pause) || state.Equals(State.End);
            background.Draw(spriteBatch, isDisabled);
            terrainManager.Draw(spriteBatch, isDisabled);
            characterManager.Draw(spriteBatch, isDisabled);
            explosion.Draw(spriteBatch, isDisabled);
            config.Draw(spriteBatch,isDisabled);

        }

        public override void SetEnabled(bool isEnabled)
        {
            base.SetEnabled(isEnabled);
            state = isEnabled ? State.Processing : State.Pause;
        }
    }
}
