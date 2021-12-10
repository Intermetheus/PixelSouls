using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
     static class Stage
    {
        //public static int currentStage;   // Value used to track the current stage. Currently only one stage implemented
        private static Rectangle worldSize;
        private static Texture2D backgroundImage;

        public static Rectangle WorldSize { get => worldSize; set => worldSize = value; }

        public static void LoadContent(ContentManager content)
        {
            backgroundImage = content.Load<Texture2D>("backgroundImage");
        }

        public static void LoadLevel(string levelName)
        {
            // TODO: make this dynamic
            // Use string to call class methods
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
