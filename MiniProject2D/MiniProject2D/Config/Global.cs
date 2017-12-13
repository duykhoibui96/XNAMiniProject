using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MiniProject2D.Config
{
    class Global
    {
        public static Global Instance;

        static Global()
        {
            Instance = new Global();
        }

        private Global()
        {
            rand = new Random();
            mapWidth = 18;
            mapHeight = 11;
            unit = 50;
            velocity = 2;

        }

        private int mapWidth;
        private int mapHeight;
        private Random rand;
        private int unit;
        private int velocity;
        private SpriteBatch renderComponent;
        private GraphicsDevice graphics;


        public int MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }

        public Random Rand
        {
            get { return rand; }
            set { rand = value; }
        }

        public int Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public int Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public SpriteBatch RenderComponent
        {
            get { return renderComponent; }
            set { renderComponent = value; }
        }

        public GraphicsDevice Graphics
        {
            get { return graphics; }
            set { graphics = value; }
        }
    }
}
