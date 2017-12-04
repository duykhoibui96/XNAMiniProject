using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.Config
{
    class Setting
    {
        public Color PlayerColor;
        public Color MummyColor;
        public Color ScorpionColor;
        public Color ZombieColor;


        public static Setting Instance;

        static Setting()
        {
            Instance = new Setting();
        }

        private Setting()
        {
            PlayerColor = Color.White;
            MummyColor = Color.Green;
            ScorpionColor = Color.Red;
            ZombieColor = Color.GreenYellow;
        }

    }
}
