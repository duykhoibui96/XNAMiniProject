using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Entity;
using MiniProject2D.EventHandler;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.UI
{
    class SettingUI : GameUI
    {
        private Button apply;
        private Button cancel;
        private VolumnControl musicVolumnControl;
        private VolumnControl soundVolumnControl;
        private CharacterSelector playerSelector;
        private CharacterSelector mummySelector;
        private CharacterSelector scorpionSelector;
        private CharacterSelector zombieSelector;
        private _2DModel title;

        private class VolumnControl : _2DEntity
        {
            public enum Type
            {
                Music = 0,
                Sound = 1
            }

            private Type type;
            private float vol;
            private _2DModel title;
            private _2DModel progress;
            private _2DModel control;
            private Point startPos;
            private Point endPos;

            public VolumnControl(Point startPos, Type type)
            {
                this.type = type;
                var unit = Global.Instance.Unit;
                this.startPos = new Point(startPos.X + unit * 5, startPos.Y);
                var graphics = Global.Instance.Graphics;
                var progressTexture = new Texture2D(graphics, 1, 1);
                endPos = new Point(this.startPos.X + unit * 10, this.startPos.Y);
                progressTexture.SetData(new Color[]
                {
                    Color.Red
                });
                string titleText = "";
                switch (type)
                {
                    case Type.Music:
                        vol = SoundManager.Instance.MusicVolumn;
                        titleText = "Music volumn";
                        break;
                    case Type.Sound:
                        vol = SoundManager.Instance.SoundVolumn;
                        titleText = "Sound volumn";
                        break;
                }
                var progressLength = (int)(vol * unit * 10);
                title = new _2DModel(titleText, ResManager.Instance.SmallNotifyFont, new Vector2(startPos.X, startPos.Y), Color.Blue);
                progress = new _2DModel(progressTexture, new Rectangle(this.startPos.X, this.startPos.Y + unit / 4, progressLength, unit / 2), Color.White);
                control = new _2DModel(ResManager.Instance.Control, new Rectangle(this.startPos.X + progressLength, this.startPos.Y, unit, unit), Color.White);



            }

            public void Update()
            {
                var mouse = MouseEvent.Instance;
                if (mouse.IsHoldLeftMouse && control.Rect.Contains(mouse.MousePosition))
                {
                    var unit = Global.Instance.Unit;
                    var currentMousePosX = mouse.MousePosition.X;
                    var previousMousePosX = mouse.PreviousMousePosition.X;
                    var offset = currentMousePosX - previousMousePosX;
                    var controlRect = control.Rect;
                    var progressRect = progress.Rect;
                    controlRect.X += offset;
                    if (controlRect.X < startPos.X || controlRect.X > endPos.X) return;
                    progressRect.Width = controlRect.X - startPos.X;
                    control.Rect = controlRect;
                    progress.Rect = progressRect;

                    vol = (float)progressRect.Width / (unit * 10);
                    Synchronize();
                }

            }

            private void Synchronize()
            {
                switch (type)
                {
                    case Type.Music:
                        SoundManager.Instance.MusicVolumn = vol;
                        break;
                    case Type.Sound:
                        SoundManager.Instance.SoundVolumn = vol;
                        break;
                }
            }

            public override _2DModel[] Models
            {
                get
                {
                    return new _2DModel[]
                    {
                        title,
                        progress,
                        control
                    };
                }
            }
        }

        private class CharacterSelector : _2DEntity
        {
            public enum Type
            {
                Player = 0,
                Mummy = 1,
                Scorpion = 2,
                Zombie = 3
            }

            private Type type;
            private _2DModel title;
            private _2DModel[] characters;
            private _2DModel selectedLine;
            private int selectedCharacter;

            public CharacterSelector(Point pos, Type type)
            {
                var unit = Global.Instance.Unit;
                var graphics = Global.Instance.Graphics;
                var lineTexture = new Texture2D(graphics, 1, 1);
                lineTexture.SetData(new Color[]
                {
                    Color.Blue
                });
                this.type = type;
                Texture2D texture = ResManager.Instance.Player;
                int numbersOfFrame = 4, defaultFrameIndex = 2;
                Color characterColor = Character.PlayerColor;
                string title = "Player style";
                switch (type)
                {
                    case Type.Mummy:
                        texture = ResManager.Instance.Mummy;
                        numbersOfFrame = 3;
                        defaultFrameIndex = 1;
                        characterColor = Character.MummyColor;
                        title = "Mummy style";
                        break;
                    case Type.Scorpion:
                        texture = ResManager.Instance.Scorpion;
                        numbersOfFrame = 4;
                        defaultFrameIndex = 2;
                        characterColor = Character.ScorpionColor;
                        title = "Scorpion style";
                        break;
                    case Type.Zombie:
                        texture = ResManager.Instance.Zombie;
                        numbersOfFrame = 6;
                        defaultFrameIndex = 0;
                        characterColor = Character.ZombieColor;
                        title = "Zombie style";
                        break;
                }

                var sourceRect = new Rectangle((texture.Width / numbersOfFrame) * defaultFrameIndex, 0,
                    texture.Width / numbersOfFrame, texture.Height / 4);

                this.title = new _2DModel(title, ResManager.Instance.SmallNotifyFont, new Vector2(pos.X, pos.Y), Color.Red);
                characters = new _2DModel[]
                {
                    new _2DModel(texture, new Rectangle(pos.X + unit * 5, pos.Y,unit,unit),Color.White,sourceRect), 
                    new _2DModel(texture, new Rectangle(pos.X + unit * 9, pos.Y,unit,unit),Color.Green,sourceRect), 
                    new _2DModel(texture, new Rectangle(pos.X + unit * 13, pos.Y,unit,unit),Color.Blue,sourceRect) 
                };

                for (int i = 0; i < 3; i++)
                {
                    if (characters[i].CurrentColor.Equals(characterColor))
                    {
                        selectedCharacter = i;
                        var rect = characters[i].Rect;
                        selectedLine = new _2DModel(lineTexture, new Rectangle(rect.Left, rect.Bottom + 5, rect.Width, 5), Color.White);
                        break;
                    }
                }

            }

            public void Synchronize()
            {
                switch (type)
                {
                    case Type.Player:
                        Character.PlayerColor = characters[selectedCharacter].CurrentColor;
                        break;
                    case Type.Mummy:
                        Character.MummyColor = characters[selectedCharacter].CurrentColor;
                        break;
                    case Type.Scorpion:
                        Character.ScorpionColor = characters[selectedCharacter].CurrentColor;
                        break;
                    case Type.Zombie:
                        Character.ZombieColor = characters[selectedCharacter].CurrentColor;
                        break;
                }
            }

            public void Update()
            {
                if (MouseEvent.Instance.IsLeftClick)
                {
                    var mousePos = MouseEvent.Instance.MousePosition;
                    for (int i = 0; i < 3; i++)
                    {
                        if (characters[i].Rect.Contains(mousePos))
                        {
                            selectedCharacter = i;
                            var characterRect = characters[i].Rect;
                            var lineRect = selectedLine.Rect;
                            lineRect.X = characterRect.Left;
                            selectedLine.Rect = lineRect;
                            break;
                        }
                    }
                }
            }

            public override _2DModel[] Models
            {
                get
                {
                    var modelList = new List<_2DModel>()
                    {
                        selectedLine,
                        title
                    };
                    modelList.AddRange(characters);

                    return modelList.ToArray();
                }
            }
        }

        public SettingUI()
        {
            var unit = Global.Instance.Unit;
            SoundManager.Instance.Backup();
            title = new _2DModel("SETTING", ResManager.Instance.NotifyFont, new Vector2(unit * 2, unit / 2), Color.Red);
            musicVolumnControl = new VolumnControl(new Point(unit * 2, unit * 2), VolumnControl.Type.Music);
            soundVolumnControl = new VolumnControl(new Point(unit * 2, unit * 4), VolumnControl.Type.Sound);
            playerSelector = new CharacterSelector(new Point(unit * 2, unit * 6), CharacterSelector.Type.Player);
            mummySelector = new CharacterSelector(new Point(unit * 2, unit * 8), CharacterSelector.Type.Mummy);
            scorpionSelector = new CharacterSelector(new Point(unit * 2, unit * 10), CharacterSelector.Type.Scorpion);
            zombieSelector = new CharacterSelector(new Point(unit * 2, unit * 12), CharacterSelector.Type.Zombie);

            apply = new Button("APPLY", new Point(unit * 20, unit * 3), EventBoard.Event.ApplySetting);
            cancel = new Button("CANCEL", new Point(unit * 20, unit * 6), EventBoard.Event.CancelSetting);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            apply.Interact();
            cancel.Interact();
            musicVolumnControl.Update();
            soundVolumnControl.Update();
            playerSelector.Update();
            mummySelector.Update();
            scorpionSelector.Update();
            zombieSelector.Update();
        }

        public override void Render()
        {
            var modelList = new List<_2DModel>()
            {
                title
            };
            modelList.AddRange(apply.Models);
            modelList.AddRange(cancel.Models);
            modelList.AddRange(musicVolumnControl.Models);
            modelList.AddRange(soundVolumnControl.Models);
            modelList.AddRange(playerSelector.Models);
            modelList.AddRange(mummySelector.Models);
            modelList.AddRange(scorpionSelector.Models);
            modelList.AddRange(zombieSelector.Models);

            _2DModel.Render(modelList.ToArray());
        }

        protected override void HandleEvent()
        {
            var ev = EventBoard.Instance.GetEvent();
            var eventHandled = true;

            switch (ev)
            {
                case EventBoard.Event.CancelSetting:
                    SoundManager.Instance.Recover();
                    break;
                case EventBoard.Event.ApplySetting:
                    playerSelector.Synchronize();
                    mummySelector.Synchronize();
                    scorpionSelector.Synchronize();
                    zombieSelector.Synchronize();
                    break;
                default:
                    eventHandled = false;
                    break;
            }

            if (eventHandled)
            {
                EventBoard.Instance.Finish();
                EventBoard.Instance.AddEvent(EventBoard.Event.CloseSetting);
            }

        }
    }
}
