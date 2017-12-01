using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.EventHandler;
using MiniProject2D.GameComponent;
using MiniProject2D.MenuComponent;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Vector2 notifyPos;
        private SpriteFont notifyFont;
        private List<GameView> gameViews;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 600,
                PreferredBackBufferWidth = 1200
            };

            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            ResManager.Instance.InitComponents(this);
            var match = new GameMatch(this) { IsVisible = false };
            match.Init();
            var pauseView = new GamePause(this) { IsVisible = false };
            var menuView = new MenuView(this);
            var winView = new WinnerView(this) { IsVisible = false };
            var loseView = new LoserView(this) { IsVisible = false };
            gameViews = new List<GameView>()
            {
                menuView,
                match,
                pauseView,
                winView,
                loseView
            };

            SoundManager.Instance.PlayMenuMusic();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            UserInput.Instance.Update();
            if (UserInput.Instance.PressedKey.Equals(Keys.P))
                EventBoard.Instance.Ev = EventBoard.Event.PauseGame;
            else if (UserInput.Instance.PressedKey.Equals(Keys.R))
                EventBoard.Instance.Ev = EventBoard.Event.ResumeGame;

            EventHandler(EventBoard.Instance.Ev);
            foreach (var view in gameViews)
            {
                view.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void EventHandler(EventBoard.Event ev)
        {
            switch (ev)
            {
                case EventBoard.Event.PauseGame:
                    foreach (var view in gameViews)
                    {
                        switch (view.Type)
                        {
                            case GameView.ViewType.Match:
                                view.SetEnabled(false);
                                break;
                            case GameView.ViewType.Pause:
                                view.IsVisible = true;
                                break;
                            default:
                                view.IsVisible = false;
                                break;
                        }
                    }
                    break;
                case EventBoard.Event.ResumeGame:
                    foreach (var view in gameViews)
                    {
                        if (view.Type == GameView.ViewType.Match)
                        {
                            view.SetEnabled(true);
                        }
                        else
                            view.IsVisible = false;
                    }
                    break;
                case EventBoard.Event.StartGame:
                    SoundManager.Instance.StopPlayingMenuMusic();
                    SoundManager.Instance.PlayGameMusic();
                    foreach (var view in gameViews)
                    {
                        if (view.Type == GameView.ViewType.Match)
                        {
                            view.IsVisible = true;
                            ((GameMatch)view).Init();
                        }
                        else
                        {
                            view.IsVisible = false;
                        }
                    }
                    break;
                case EventBoard.Event.ReturnToMenu:
                    SoundManager.Instance.StopPlayingGameMusic();
                    SoundManager.Instance.PlayMenuMusic();
                    foreach (var view in gameViews)
                    {
                        view.IsVisible = view.Type == GameView.ViewType.Menu;
                    }
                    break;
                case EventBoard.Event.ShowResultsWhenLose:
                    foreach (var view in gameViews)
                    {
                        switch (view.Type)
                        {
                            case GameView.ViewType.Match:
                                view.SetEnabled(false);
                                break;
                            case GameView.ViewType.Win:
                                view.IsVisible = true;
                                break;
                            default:
                                view.IsVisible = false;
                                break;
                        }
                    }
                    break;
                case EventBoard.Event.ShowResultsWhenWin:
                    foreach (var view in gameViews)
                    {
                        switch (view.Type)
                        {
                            case GameView.ViewType.Match:
                                view.SetEnabled(false);
                                break;
                            case GameView.ViewType.Lose:
                                view.IsVisible = true;
                                break;
                            default:
                                view.IsVisible = false;
                                break;
                        }
                    }
                    break;
                case EventBoard.Event.Exit:
                    this.Exit();
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Turquoise);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var view in gameViews)
            {
                view.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
