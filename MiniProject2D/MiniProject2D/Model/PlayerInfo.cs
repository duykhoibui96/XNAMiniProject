using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniProject2D.Config;
using MiniProject2D.Resource;

namespace MiniProject2D.Model
{
    class PlayerInfo
    {
        private class Record
        {
            private string playerName;
            private string playerScore;
            private bool isTitle;
            private Vector2 pos;
            private Texture2D line;
            private Rectangle lineRect;

            public Record(string playerName, string playerScore, bool isTitle = false)
            {
                this.playerName = playerName;
                this.playerScore = playerScore;
                this.isTitle = isTitle;
                if (isTitle)
                {
                    line = new Texture2D(Setting.Instance.Graphics,1,1);
                    line.SetData(new Color[]
                    {
                        Color.Red
                    });
                }
            }

            public void ApplyPosition(int startX, int startY)
            {
                pos = new Vector2(startX, startY);
                if (isTitle)
                {
                    lineRect = new Rectangle((int) pos.X,(int) pos.Y + 40,500,5);
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                var color = isTitle ? Color.Red : Color.White;
                var spriteFont = ResManager.Instance.SmallNotifyFont;
                var unit = Configuration.Unit;
                spriteBatch.DrawString(spriteFont, playerName, pos, color);
                var newPos = pos;
                newPos.X += unit * 5;
                spriteBatch.DrawString(spriteFont, playerScore, newPos, color);
                if (isTitle)
                {
                    spriteBatch.Draw(line,lineRect,Color.White);
                }
            }
        }

        private Record[] playerRecords;

        public void Init(int startX, int startY)
        {
            var unit = Configuration.Unit;
            playerRecords = new Record[]
            {
                new Record("Player","Score",true),
                new Record("Tony",1050.ToString()),
                new Record("Jack",300.ToString()),
                new Record("Bob",200.ToString()),
                new Record("Kate",150.ToString()),
                new Record("Boss",50.ToString()),
            };

            foreach (var playerRecord in playerRecords)
            {
                playerRecord.ApplyPosition(startX, startY);
                startY += unit;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var playerRecord in playerRecords)
            {
                playerRecord.Draw(spriteBatch);
            }
        }

    }
}
