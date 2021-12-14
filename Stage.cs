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

        //WorldSize is a rectangle used for the boundaries of the playable area.
        public static Rectangle WorldSize { get => worldSize; set => worldSize = value; }

        /// <summary>
        /// Loads the floor sprite, because the sprite must be loaded, before it can be instantiated correctly.
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public static void LoadContent(ContentManager content)
        {
            backgroundImage = content.Load<Texture2D>("backgroundImage");
        }

        /// <summary>
        /// Loads a level in with a simple if statement
        /// <para>Level loading can be made dynamic in the future</para>
        /// </summary>
        /// <param name="levelName">The levelName must be a class, so that the method is ready to be made dynamic</param>
        public static void LoadLevel(string levelName)
        {
            // TODO: make this dynamic
            // Use string to call class methods
            if (levelName == "PrototypePlayground")
            {
                PrototypePlayground.CreateLevel(backgroundImage);
            }
        }

        /// <summary>
        /// When more levels are created, this should destroy() the floor and other gameObjects.
        /// </summary>
        public static void UnloadLevel()
        {

        }
    }
}
