using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.Entity;
using MiniProject2D.EventHandler;
using MiniProject2D.Information;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.UI
{
    class WinnerUI : GameUI
    {
        private _2DModel title;
        private _2DModel score;
        private _2DModel numOfTreasure;
        private _2DModel treasureWeight;

        private _2DModel[] recordList;
        private _2DModel recordTitle;
        private _2DModel playerNameTitle;
        private _2DModel scoreTitle;
        private Button save;

        private bool isSaveRecord;

        public WinnerUI()
        {
            var unit = Global.Instance.Unit;
            var graphics = Global.Instance.Graphics;

            var text = "CONGRATULATION! YOU'VE ESCAPED!";
            var titleStringLength = ResManager.Instance.NotifyFont.MeasureString(text);
            var titlePos = new Vector2(unit * 10 + (graphics.Viewport.Width - unit * 10 - titleStringLength.X) / 2, unit);
            title = new _2DModel(text, ResManager.Instance.NotifyFont, titlePos, Color.Red);

            score = new _2DModel("YOU SCORE: ", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 11, unit * 3), Color.Blue);
            numOfTreasure = new _2DModel("TREASURE QUANLITY: ", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 11, unit * 4), Color.Blue);
            treasureWeight = new _2DModel("TREASURE WEIGHT: ", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 11, unit * 5), Color.Blue);
            save = new Button("SAVE", new Point(unit * 21, unit * 3), EventBoard.Event.SaveRecord);

            text = "TOP PLAYERS";
            titleStringLength = ResManager.Instance.NotifyFont.MeasureString(text);
            titlePos = new Vector2(unit * 10 + (graphics.Viewport.Width - unit * 10 - titleStringLength.X) / 2, unit * 6);
            recordTitle = new _2DModel(text, ResManager.Instance.NotifyFont, titlePos, Color.Red);
            playerNameTitle = new _2DModel("Player name", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 13, unit * 7), Color.Red);
            scoreTitle = new _2DModel("Player score", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 23, unit * 7), Color.Red);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            save.Interact();
        }

        public override void Render()
        {
            var modelList = new List<_2DModel>()
            {
                title,
                score,
                numOfTreasure,
                treasureWeight
            };

            modelList.AddRange(save.Models);
            modelList.Add(recordTitle);
            modelList.Add(playerNameTitle);
            modelList.Add(scoreTitle);
            modelList.AddRange(recordList);

            _2DModel.Render(modelList.ToArray());
        }

        protected override void HandleEvent()
        {
            if (EventBoard.Instance.GetEvent() == EventBoard.Event.SaveRecord)
            {
                string notifyText = "";
                if (!isSaveRecord)
                {
                    var result = GameResult.Instance;
                    RecordLoader.Instance.WriteRecord(result.PlayerName, result.Score);
                    EventBoard.Instance.Finish();
                    notifyText = "Save record successfully!";
                    isSaveRecord = true;
                }
                else
                {
                    notifyText = "Record was saved before!";
                }
                
                EventBoard.Instance.AddEvent(EventBoard.Event.ShowNotification);
                EventBoard.Instance.NotifyText = notifyText;
            }
        }

        public void UpdateInformation()
        {
            var unit = Global.Instance.Unit;
            score.Text += GameResult.Instance.Score;
            RecordLoader.Instance.LoadRecords();
            var records = RecordLoader.Instance.GetTop(5);
            var recordList = new List<_2DModel>();

            var playerNamePos = playerNameTitle.Pos;
            var scorePos = scoreTitle.Pos;

            foreach (var record in records)
            {
                playerNamePos.Y += unit;
                scorePos.Y += unit;

                recordList.Add(new _2DModel(record.PlayerName, ResManager.Instance.SmallNotifyFont, playerNamePos, Color.Green));
                recordList.Add(new _2DModel(record.Score.ToString(), ResManager.Instance.SmallNotifyFont, scorePos, Color.Green));
            
            }

            this.recordList = recordList.ToArray();
        }
    }
}
