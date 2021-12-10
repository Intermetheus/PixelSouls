using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    static class PrototypePlayground
    {
        public static void CreateLevel(Texture2D sprite)
        {
            // Change boundary size for collision
            Stage.WorldSize = new Rectangle(0, 0, 2000, 2000);

            // Sprite is send from stage, where it gets loaded.
            SetBackground(sprite);

            // TODO: Instantiate bosses, enemies etc.
        }

        private static void SetBackground(Texture2D sprite)
        {
            // Draw Floor
            for (int i = 0; i <= Stage.WorldSize.Width / 800 + 1; i++)
            {
                for (int j = 0; j <= Stage.WorldSize.Height / 800 + 1; j++)
                {
                    GameWorld.Instantiate(new Floor(i * 800, j * 800, sprite));
                }
            }

            // Draw walls
            // Walls to be implemented
        }
    }
}
