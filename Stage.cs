using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
     class Stage
    {
        public static int currentStage;
        private static Rectangle worldSize;
        protected static Texture2D backgroundImage;


        public static Rectangle WorldSize { get => worldSize; set => worldSize = value; }

        public static void loadContent(ContentManager content)
        {
            backgroundImage = content.Load<Texture2D>("backgroundImage");
        }


        public static void LoadLevel(string levelName)
        {
            //TODO: make this dynamic
            //Use string to call class methods
            if (levelName == "PrototypePlayground")
            {
                PrototypePlayground.CreateLevel(backgroundImage);
            }
        }

        public static void UnloadLevel()
        {

        }
    }
}
