using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private BackgroundEntity logo;
        private BackgroundEntity menuContainer;
        private ButtonEntity playAgain;
        private ButtonEntity setting;
        private ButtonEntity returnToMenu;


        private CharacterManager characterManager;
        private TerrainManager terrainManager;
        private BackgroundEntity background;
        private AnimationEntity explosion;
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

        public override void Init()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            var pos = new Vector2(unit, unit);

            var menuTexture = new Texture2D(graphicsDevice, 1, 1);
            menuTexture.SetData(new Color[]
            {
                Color.Blue
            });

            menuContainer = new BackgroundEntity(menuTexture, new Rectangle(0, 0, unit * 10, graphicsDevice.Viewport.Height), Color.White);

            logo = new BackgroundEntity(ResManager.Instance.Logo, new Rectangle((int)pos.X, (int)pos.Y, unit * 8, unit * 2), Color.White);
            pos.Y += unit * 3;
            playAgain = new ButtonEntity("PLAY AGAIN", pos, EventBoard.Event.ResetGame);
            pos.Y += unit * 3;
            setting = new ButtonEntity("SETTING", pos, EventBoard.Event.OpenSettings);
            pos.Y += unit * 3;
            returnToMenu = new ButtonEntity("RETURN TO MENU", pos, EventBoard.Event.ReturnToMenu);

            explosion = new AnimationEntity(ResManager.Instance.Collision, new Rectangle(0, 0, 100, 100), Color.White, 4, 0);
            background = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            terrainManager = new TerrainManager();
            characterManager = new CharacterManager();

            InitMapAndCharacters();

        }

        private void InitMapAndCharacters()
        {
            var unit = Configuration.Unit;

            int numbersOfObstacles, numbersOfZombie, numbersOfScorpion, numbersOfMummy;
            GetComponentQuanlities(out numbersOfObstacles, out numbersOfMummy, out numbersOfScorpion,
                out numbersOfZombie);

            state = State.Start;
            explosion.IsVisible = false;
            endGameDelayTime = 1000;

            terrainManager.Init(unit * 11, unit, numbersOfObstacles);
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

            HandledEvent();

            playAgain.Update(gameTime);
            setting.Update(gameTime);
            returnToMenu.Update(gameTime);

            switch (state)
            {
                case State.Start:
                    state = State.Processing;
                    break;
                case State.Processing:
                    characterManager.Update(gameTime);
                    if (characterManager.IsWon())
                        state = State.Win;
                    if (characterManager.CheckCollision())
                        state = State.Lose;
                    break;
                case State.Pause:
                    break;
                case State.Win:
                    EventBoard.Instance.AddEvent(EventBoard.Event.ShowResult);
                    PlayerRecord.Instance.SetResult(true, 3 * terrainManager.NumbersOfFreeSpace - characterManager.NumbersOfPlayerSteps);
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
                        PlayerRecord.Instance.SetResult(false);
                    }
                    explosion.Update(gameTime);
                    endGameDelayTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (endGameDelayTime <= 0)
                    {
                        EventBoard.Instance.AddEvent(EventBoard.Event.ShowResult);
                        state = State.Pause;
                        mode = ViewMode.DISABLED;
                    }
                    break;
            }


        }

        private void HandledEvent()
        {
            if (EventBoard.Instance.GetEvent() == EventBoard.Event.ResetGame)
            {
                InitMapAndCharacters();
                EventBoard.Instance.Finish();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == ViewMode.INVISIBLE) return;
            var isDisabled = mode == ViewMode.DISABLED;
            background.Draw(spriteBatch, isDisabled);

            menuContainer.Draw(spriteBatch, isDisabled);
            logo.Draw(spriteBatch);
            playAgain.Draw(spriteBatch, isDisabled);
            setting.Draw(spriteBatch, isDisabled);
            returnToMenu.Draw(spriteBatch, isDisabled);

            terrainManager.Draw(spriteBatch, isDisabled);
            characterManager.Draw(spriteBatch, isDisabled);
            explosion.Draw(spriteBatch, isDisabled);

        }

    }
}
