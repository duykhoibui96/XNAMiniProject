using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.Resource
{
    class ResManager
    {
        public Texture2D Wall { get; private set; }
        public Texture2D Ground { get; private set; }
        public Texture2D Dialog { get; private set; }
        public Texture2D Light { get; private set; }
        public Texture2D Boundary { get; private set; }
        public Texture2D MenuBackground { get; private set; }
        public Texture2D ArrowLeft { get; private set; }
        public Texture2D ArrowRight { get; private set; }
        public Texture2D ArrowUp { get; private set; }
        public Texture2D ArrowDown { get; private set; }
        public Texture2D Player { get; private set; }
        public Texture2D Zombie { get; private set; }
        public Texture2D Mummy { get; private set; }
        public Texture2D Scorpion { get; private set; }
        public Texture2D Collision { get; private set; }
        public Texture2D Flaming { get; private set; }
        public Texture2D Config { get; private set; }
        public Texture2D ConfigHover { get; private set; }
        public Texture2D Resume { get; private set; }
        public Texture2D ResumeHover { get; private set; }
        public Texture2D Settings { get; private set; }
        public Texture2D SettingsHover { get; private set; }
        public Texture2D ReturnToMenu { get; private set; }
        public Texture2D ReturnToMenuHover { get; private set; }
        public Texture2D NewGame { get; private set; }
        public Texture2D NewGameHover { get; private set; }
        public Texture2D ResetMatch { get; private set; }
        public Texture2D ResetMatchHover { get; private set; }
        public Texture2D Exit { get; private set; }
        public Texture2D ExitHover { get; private set; }
     
        public SpriteFont NotifyFont { get; private set; }
        public Texture2D Vision { get; private set; }

        public SoundEffect GameMusic { get; private set; }
        public SoundEffect MonsterEncounter { get; private set; }
        public SoundEffect Explosion { get; private set; }
        public SoundEffect MenuMusic{ get; private set; }
        public SoundEffect FootSteps { get; private set; }
        public SoundEffect WinSound { get; private set; }
        public SoundEffect LoseSound { get; private set; }
        public SoundEffect ClickSound { get; private set; }
        public SoundEffect HoverSound { get; private set; }



        public void InitComponents(Game game)
        {
            Wall = game.Content.Load<Texture2D>("Map/wall");
            Ground = game.Content.Load<Texture2D>("Map/ground");
            Dialog = game.Content.Load<Texture2D>("Others/dialog");
            Light = game.Content.Load<Texture2D>("Effect/light");
            Boundary = game.Content.Load<Texture2D>("Map/boundary");
            MenuBackground = game.Content.Load<Texture2D>("Others/menu-background");
            ArrowLeft = game.Content.Load<Texture2D>("Map/arrow-left");
            ArrowRight = game.Content.Load<Texture2D>("Map/arrow-right");
            ArrowUp = game.Content.Load<Texture2D>("Map/arrow-up");
            ArrowDown = game.Content.Load<Texture2D>("Map/arrow-down");
            Player = game.Content.Load<Texture2D>("Character/playerAnim");
            Zombie = game.Content.Load<Texture2D>("Character/zombie");
            Mummy = game.Content.Load<Texture2D>("Character/mummy");
            Scorpion = game.Content.Load<Texture2D>("Character/scorpion");
            Collision = game.Content.Load<Texture2D>("Effect/collision");
            Flaming = game.Content.Load<Texture2D>("Effect/flaming");
            Config = game.Content.Load<Texture2D>("Button/config");
            ConfigHover = game.Content.Load<Texture2D>("Button/config_hover");
            Resume = game.Content.Load<Texture2D>("Button/resume");
            ResumeHover = game.Content.Load<Texture2D>("Button/resume_hover");
            Settings = game.Content.Load<Texture2D>("Button/settings");
            SettingsHover = game.Content.Load<Texture2D>("Button/settings_hover");
            ReturnToMenu = game.Content.Load<Texture2D>("Button/return-to-menu");
            ReturnToMenuHover = game.Content.Load<Texture2D>("Button/return-to-menu_hover");
            NewGame = game.Content.Load<Texture2D>("Button/new-game");
            NewGameHover = game.Content.Load<Texture2D>("Button/new-game_hover");
            Exit = game.Content.Load<Texture2D>("Button/exit");
            ExitHover = game.Content.Load<Texture2D>("Button/exit_hover");
            ResetMatch = game.Content.Load<Texture2D>("Button/reset-match");
            ResetMatchHover = game.Content.Load<Texture2D>("Button/reset-match_hover");
            NotifyFont = game.Content.Load<SpriteFont>("Font/GameFont");
            Vision = game.Content.Load<Texture2D>("Others/vision");
            GameMusic = game.Content.Load<SoundEffect>("Sound/game-music");
            MenuMusic = game.Content.Load<SoundEffect>("Sound/menu-music");
            MonsterEncounter = game.Content.Load<SoundEffect>("Sound/monster-encounter");
            Explosion = game.Content.Load<SoundEffect>("Sound/explosion");
            FootSteps = game.Content.Load<SoundEffect>("Sound/footsteps");
            WinSound = game.Content.Load<SoundEffect>("Sound/win");
            LoseSound = game.Content.Load<SoundEffect>("Sound/lose");
            ClickSound = game.Content.Load<SoundEffect>("Sound/click");
            HoverSound = game.Content.Load<SoundEffect>("Sound/hover");
        }

        public static ResManager Instance;

        static ResManager()
        {
            Instance = new ResManager();
        }

    }
}
