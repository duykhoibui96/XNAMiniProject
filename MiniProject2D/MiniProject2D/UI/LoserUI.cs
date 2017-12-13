using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework;
using MiniProject2D.Config;
using MiniProject2D.Information;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.UI
{
    class LoserUI : GameUI
    {
        private _2DModel title;
        private _2DModel RIPImage;
        private _2DModel deathReason;

        public LoserUI()
        {
            var unit = Global.Instance.Unit;
            var graphics = Global.Instance.Graphics;
            var text = "GAME OVER!";
            var titleStringLength = ResManager.Instance.NotifyFont.MeasureString(text);
            var titlePos = new Vector2(unit * 10 + (graphics.Viewport.Width - unit * 10 - titleStringLength.X) / 2, unit);
            title = new _2DModel(text, ResManager.Instance.NotifyFont, titlePos, Color.Blue);

            RIPImage = new _2DModel(ResManager.Instance.Rip, new Rectangle(unit * 11, unit * 3, unit * 6, unit * 6), Color.White);
            deathReason = new _2DModel("You've been killed by monster", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 18, unit * 4), Color.Red);
        }

        public override void Render()
        {
            _2DModel.Render(new _2DModel[]
            {
                title,
                RIPImage,
                deathReason
            });
        }

        protected override void HandleEvent()
        {

        }

        public void UpdateInformation()
        {
            
        }
    }
}
