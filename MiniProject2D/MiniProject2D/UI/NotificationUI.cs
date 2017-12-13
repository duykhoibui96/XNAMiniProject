using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Entity;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.UI
{
    class NotificationUI: GameUI
    {
        private _2DModel container;
        private _2DModel notification;
        private Button ok;

        public NotificationUI(string notificationText)
        {
            var graphics = Global.Instance.Graphics;
            var unit = Global.Instance.Unit;
            var notificationSize = ResManager.Instance.NotifyFont.MeasureString(notificationText);
            var notificationPos = new Vector2((graphics.Viewport.Width - notificationSize.X)/2,
                graphics.Viewport.Height/4);
            var buttonPos = new Point(graphics.Viewport.Width/2 - unit*5,graphics.Viewport.Height/4 + unit * 2);

            notification = new _2DModel(notificationText,ResManager.Instance.NotifyFont,notificationPos,Color.Red);
            ok = new Button("OK",buttonPos,EventBoard.Event.DismissDialog);
            container = new _2DModel(ResManager.Instance.Dialog,new Rectangle(graphics.Viewport.Width / 2 - unit * 10, (int) (notificationPos.Y - unit), unit * 20, unit * 6),Color.White);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ok.Interact();
        }

        public override void Render()
        {
            var modelList = new List<_2DModel>()
            {
                container,
                notification
            };

            modelList.AddRange(ok.Models);
            _2DModel.Render(modelList.ToArray());

        }

        protected override void HandleEvent()
        {
            
        }
    }
}
