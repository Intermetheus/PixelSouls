using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
     class Stage
    {
        public static int currentStage;
        private static Rectangle worldSize;

        public static Rectangle WorldSize { get => worldSize; set => worldSize = value; }

        public static void LoadLevel(string levelName)
        {

        }

        public static void UnloadLevel()
        {

        }
    }
}
