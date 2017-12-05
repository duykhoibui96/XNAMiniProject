using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.View
{
    class SettingView : GameView
    {
        private BackgroundEntity container;

        private Texture2D border;
        private Rectangle borderRect;

        private Vector2 playerSelectTitle;
        private AnimationEntity player;
        private Rectangle[] playerRect;
        private int selectedPlayer = 0;
        private Color[] playerColor;


        private Vector2 mummySelectTitle;
        private AnimationEntity mummy;
        private Rectangle[] mummyRect;
        private int selectedMummy = 0;
        private Color[] mummyColor;

        private Vector2 scorpionSelectTitle;
        private AnimationEntity scorpion;
        private Rectangle[] scorpionRect;
        private int selectedScorpion = 0;
        private Color[] scorpionColor;

        private Vector2 zombieSelectTitle;
        private AnimationEntity zombie;
        private Rectangle[] zombieRect;
        private int selectedZombie = 0;
        private Color[] zombieColor;

        // private ButtonEntity ok;
        private ButtonEntity apply;
        private ButtonEntity cancel;


        private Vector2 settingPosition;

        private BackgroundEntity musicVolumnControl;
        private BackgroundEntity musicVolumnProgress;
        private Vector2 musicVolumnTitle;
        private Vector2 musicVolumnNumberPos;
        private int musicVolumn = 0; //0 -> 100

        private BackgroundEntity soundVolumnControl;
        private BackgroundEntity soundVolumnProgress;
        private Vector2 soundVolumnTitle;
        private Vector2 soundVolumnNumberPos;
        private int soundVolumn = 0; //0 -> 100

        public SettingView()
            : base()
        {
            Type = ViewType.SettingView;
        }

        public override void Init()
        {
            var graphicsDevice = Setting.Instance.Graphics;
            var unit = Configuration.Unit;
            int width = graphicsDevice.Viewport.Width, height = graphicsDevice.Viewport.Height;
            int startX = 0, startY = 0;
            int middle = startX + width / 2;

            var containerTexture = new Texture2D(graphicsDevice, 1, 1);
            containerTexture.SetData(new Color[]
            {
                Color.LightBlue
            });
            container = new BackgroundEntity(containerTexture, new Rectangle(startX, startY, width, height), Color.White);
            settingPosition = new Vector2(middle - ResManager.Instance.NotifyFont.MeasureString("SETTING").X / 2, startY + 20);
            playerSelectTitle = new Vector2(startX + 100, startY + 100);

            SoundManager.Instance.Backup();
            musicVolumnTitle = new Vector2(startX + 850, startY + 100);
            musicVolumn = (int)(SoundManager.Instance.MusicVolumn * 100);
            InitVolumn(graphicsDevice, 850, 150, musicVolumn, out musicVolumnControl, out musicVolumnProgress, out musicVolumnNumberPos);

            soundVolumnTitle = new Vector2(startX + 850, startY + 250);
            soundVolumn = (int)(SoundManager.Instance.SoundVolumn * 100);
            InitVolumn(graphicsDevice, 850, 300, soundVolumn, out soundVolumnControl, out soundVolumnProgress, out soundVolumnNumberPos);

            player = new AnimationEntity(ResManager.Instance.Player, Rectangle.Empty, Color.White, 4, 2);
            playerRect = new Rectangle[]
            {
                new Rectangle(startX + 500, startY + 100, unit, unit),
                new Rectangle(startX + 600, startY + 100, unit, unit), 
                new Rectangle(startX + 700, startY + 100, unit, unit)
            };
            playerColor = new Color[]
            {
                Color.White,
                Color.Red,
                Color.Green
            };
            for (int i = 0; i < 3; i++)
            {
                if (playerColor[i] == Setting.Instance.PlayerColor)
                {
                    selectedPlayer = i;
                    break;
                }
            }

            mummySelectTitle = new Vector2(startX + 100, startY + 200);
            mummy = new AnimationEntity(ResManager.Instance.Mummy, Rectangle.Empty, Color.White, 3, 1);
            mummyRect = new Rectangle[]
            {
                new Rectangle(startX + 500, startY + 200, unit, unit),
                new Rectangle(startX + 600, startY + 200, unit, unit), 
                new Rectangle(startX + 700, startY + 200, unit, unit)
            };
            mummyColor = new Color[]
            {
                Color.White,
                Color.Purple,
                Color.Teal
            };
            for (int i = 0; i < 3; i++)
            {
                if (mummyColor[i] == Setting.Instance.MummyColor)
                {
                    selectedMummy = i;
                    break;
                }
            }

            scorpionSelectTitle = new Vector2(startX + 100, startY + 300);
            scorpion = new AnimationEntity(ResManager.Instance.Scorpion, Rectangle.Empty, Color.White, 4, 2);
            scorpionRect = new Rectangle[]
            {
                new Rectangle(startX + 500, startY + 300, unit, unit),
                new Rectangle(startX + 600, startY + 300, unit, unit), 
                new Rectangle(startX + 700, startY + 300, unit, unit)
            };
            scorpionColor = new Color[]
            {
                Color.White,
                Color.Tan,
                Color.DimGray
            };
            for (int i = 0; i < 3; i++)
            {
                if (scorpionColor[i] == Setting.Instance.ScorpionColor)
                {
                    selectedScorpion = i;
                    break;
                }
            }

            zombieSelectTitle = new Vector2(startX + 100, startY + 400);
            zombie = new AnimationEntity(ResManager.Instance.Zombie, Rectangle.Empty, Color.White, 6, 0);
            zombieRect = new Rectangle[]
            {
                new Rectangle(startX + 500, startY + 400, unit, unit),
                new Rectangle(startX + 600, startY + 400, unit, unit), 
                new Rectangle(startX + 700, startY + 400, unit, unit)
            };
            zombieColor = new Color[]
            {
                Color.White,
                Color.DarkTurquoise,
                Color.Brown
            };
            for (int i = 0; i < 3; i++)
            {
                if (zombieColor[i] == Setting.Instance.ZombieColor)
                {
                    selectedZombie = i;
                    break;
                }
            }

            border = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            border.SetData(new Color[] { Color.Purple });
            borderRect = new Rectangle(0, 0, 60, 60);

            apply = new ButtonEntity("APPLY", new Vector2(startX + width / 6 - unit, startY + height - 150), EventBoard.Event.ApplySetting);
            // ok = new ButtonEntity("OK", new Vector2(startX + width / 2, startY + height - 100), Color.Yellow, EventBoard.Event.ApplySpriteToSetting);
            cancel = new ButtonEntity("CANCEL", new Vector2(startX + width / 2 + unit, startY + height - 150), EventBoard.Event.CancelSetting);
        }

        private static void InitVolumn(GraphicsDevice graphicsDevice, int startX, int startY, int volumn, out BackgroundEntity volumnControl, out BackgroundEntity volumnProgress, out Vector2 volumnIndexPos)
        {
            var progress = new Texture2D(graphicsDevice, 1, 1);
            progress.SetData(new Color[]
            {
                Color.Red
            });
            volumnProgress = new BackgroundEntity(progress, new Rectangle(startX, startY + 20, 200 * volumn / 100, 10), Color.White);
            volumnControl = new BackgroundEntity(ResManager.Instance.Control, new Rectangle(volumnProgress.Rect.X + volumnProgress.Rect.Width, startY, 50, 50), Color.White);
            volumnIndexPos = new Vector2(startX, startY + 50);

        }

        public override void Update(GameTime gameTime)
        {
            if (EventBoard.Instance.GetEvent() == EventBoard.Event.ApplySetting)
            {
                ApplySetting();
                return;
            }
            else if (EventBoard.Instance.GetEvent() == EventBoard.Event.CancelSetting)
            {
                CancelSetting();
                return;
            }

            apply.Update(gameTime);
            //ok.Update(gameTime);
            cancel.Update(gameTime);

            if (MouseEvent.Instance.IsLeftClick) //Choose character style
            {
                var cursorPos = MouseEvent.Instance.MousePosition;
                for (int i = 0; i < 3; i++)
                {
                    if (playerRect[i].Contains(cursorPos))
                    {
                        selectedPlayer = i;
                        break;
                    }
                    if (mummyRect[i].Contains(cursorPos))
                    {
                        selectedMummy = i;
                        break;
                    }
                    if (scorpionRect[i].Contains(cursorPos))
                    {
                        selectedScorpion = i;
                        break;
                    }
                    if (zombieRect[i].Contains(cursorPos))
                    {
                        selectedZombie = i;
                        break;
                    }
                }
            }
            else if (MouseEvent.Instance.IsHoldLeftMouse)
            {
                var previousCursorPos = MouseEvent.Instance.PreviousMousePosition;
                var cursorPos = MouseEvent.Instance.MousePosition;
                if (musicVolumnControl.Rect.Contains(cursorPos)) // Modify music volumn
                {
                    musicVolumnControl.Rect.X += (cursorPos.X - previousCursorPos.X);
                    if (musicVolumnControl.Rect.X < 850) musicVolumnControl.Rect.X = 850;
                    else if (musicVolumnControl.Rect.X > 1050) musicVolumnControl.Rect.X = 1050;
                    musicVolumnProgress.Rect.Width = musicVolumnControl.Rect.X - 850;
                    musicVolumn = musicVolumnProgress.Rect.Width * 100 / 200;
                    SoundManager.Instance.MusicVolumn = (float)musicVolumn / 100;
                }
                else if (soundVolumnControl.Rect.Contains(cursorPos))
                {
                    soundVolumnControl.Rect.X += (cursorPos.X - previousCursorPos.X);
                    if (soundVolumnControl.Rect.X < 850) soundVolumnControl.Rect.X = 850;
                    else if (soundVolumnControl.Rect.X > 1050) soundVolumnControl.Rect.X = 1050;
                    soundVolumnProgress.Rect.Width = soundVolumnControl.Rect.X - 850;
                    soundVolumn = soundVolumnProgress.Rect.Width * 100 / 200;
                    SoundManager.Instance.SoundVolumn = (float)soundVolumn / 100;
                }
            }
        }

        private void DrawBorder(SpriteBatch spriteBatch, Rectangle instance)
        {
            const int thicknessOfBorder = 5;
            var borderColor = Color.Blue;
            borderRect.X = instance.X - 5;
            borderRect.Y = instance.Y - 5;

            // Draw top line
            spriteBatch.Draw(border, new Rectangle(borderRect.X, borderRect.Y, borderRect.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(border, new Rectangle(borderRect.X, borderRect.Y, thicknessOfBorder, borderRect.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(border, new Rectangle((borderRect.X + borderRect.Width - thicknessOfBorder),
                                            borderRect.Y,
                                            thicknessOfBorder,
                                            borderRect.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(border, new Rectangle(borderRect.X,
                                            borderRect.Y + borderRect.Height - thicknessOfBorder,
                                            borderRect.Width,
                                            thicknessOfBorder), borderColor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            container.Draw(spriteBatch);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "SETTING", settingPosition, Color.Red);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "Player style", playerSelectTitle, Color.Blue);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "Mummy style", mummySelectTitle, Color.Blue);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "Scorpion style", scorpionSelectTitle, Color.Blue);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "Zombie style", zombieSelectTitle, Color.Blue);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "Music volumn", musicVolumnTitle, Color.Blue);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, "Sound volumn", soundVolumnTitle, Color.Blue);

            musicVolumnProgress.Draw(spriteBatch);
            musicVolumnControl.Draw(spriteBatch);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, musicVolumn.ToString(), musicVolumnNumberPos, Color.Red);

            soundVolumnProgress.Draw(spriteBatch);
            soundVolumnControl.Draw(spriteBatch);
            spriteBatch.DrawString(ResManager.Instance.NotifyFont, soundVolumn.ToString(), soundVolumnNumberPos, Color.Red);

            for (int i = 0; i < 3; i++)
            {

                player.Rect = playerRect[i];
                player.CurrentColor = playerColor[i];
                player.Draw(spriteBatch);
                if (selectedPlayer == i)
                {
                    DrawBorder(spriteBatch, player.Rect);
                }


                mummy.Rect = mummyRect[i];
                mummy.CurrentColor = mummyColor[i];
                mummy.Draw(spriteBatch);
                if (selectedMummy == i)
                {
                    DrawBorder(spriteBatch, mummy.Rect);
                }


                scorpion.Rect = scorpionRect[i];
                scorpion.CurrentColor = scorpionColor[i];
                scorpion.Draw(spriteBatch);
                if (selectedScorpion == i)
                {
                    DrawBorder(spriteBatch, scorpion.Rect);
                }


                zombie.Rect = zombieRect[i];
                zombie.CurrentColor = zombieColor[i];
                zombie.Draw(spriteBatch);
                if (selectedZombie == i)
                {
                    DrawBorder(spriteBatch, zombie.Rect);
                }
            }

            apply.Draw(spriteBatch);
            //ok.Draw(spriteBatch);
            cancel.Draw(spriteBatch);

        }

        private void ApplySetting()
        {
            Setting.Instance.PlayerColor = playerColor[selectedPlayer];
            Setting.Instance.MummyColor = mummyColor[selectedMummy];
            Setting.Instance.ScorpionColor = scorpionColor[selectedScorpion];
            Setting.Instance.ZombieColor = zombieColor[selectedZombie];

            EventBoard.Instance.Finish();

            EventBoard.Instance.AddEvent(EventBoard.Event.ApplySpriteToGame);
            EventBoard.Instance.AddEvent(EventBoard.Event.CloseSettings);

        }

        private void CancelSetting()
        {
            SoundManager.Instance.Recover();
            EventBoard.Instance.Finish();
            EventBoard.Instance.AddEvent(EventBoard.Event.CloseSettings);

        }

    }
}
