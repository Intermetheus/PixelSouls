using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// This class is a specific stage, that can be loaded by the Stage class.
    /// A texture for the floor is needed to laod this Stage.
    /// </summary>
    static class PrototypePlayground
    {
        /// <summary>
        /// Makes a Rectangle accessable through WorldSize, takes a sprite in parameter and sends to SetBackground()
        /// </summary>
        /// <param name="sprite">The sprite is used to draw the floors</param>
        public static void CreateLevel(Texture2D sprite)
        {
            // Change boundary size for collision
            Stage.WorldSize = new Rectangle(0, 0, 2000, 2000);
            // Sprite is send from stage, where it gets loaded.
            SetBackground(sprite);
        }

        /// <summary>
        /// Takes a sprite from CreateLevel and instantiates the floors
        /// <para>Amount of floors are dependent on the WorldSize</para>
        /// </summary>
        /// <param name="sprite">The sprite used to draw floors"</param>
        private static void SetBackground(Texture2D sprite)
        {
            // Draw Floor, divided by 800, because that is the size of the sprite, this is hardcoded, but can be changed later on.
            for (int i = 0; i <= Stage.WorldSize.Width / 800 + 1; i++)
            {
                for (int j = 0; j <= Stage.WorldSize.Height / 800 + 1; j++)
                {
                    GameWorld.Instantiate(new Floor(i * 800, j * 800, sprite));
                }
            }
        }
    }
}
