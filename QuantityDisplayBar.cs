using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Class used to display depletable colored bars on screen
    /// </summary>
    class QuantityDisplayBar
    {
        private int border = 2;
        private float percentage;
        private Vector2 position;
        private Vector2 size;
        private Color color;
        private Texture2D shade;
        private Texture2D solid;

        /// <summary>
        /// Constructor for QuantityDisplayBar
        /// </summary>
        /// <param name="content">Content reference for loading sprites</param>
        /// <param name="position">Position on screen</param>
        /// <param name="color">Color of depletable bar</param>
        /// <param name="size">Size of filled bar</param>
        public QuantityDisplayBar(ContentManager content, Vector2 position, Color color, Vector2 size)
        {
            this.position = position;
            this.color = color;
            this.size = size;

            shade = content.Load<Texture2D>("shade");
            solid = content.Load<Texture2D>("solid");
        }

        /// <summary>
        /// Updates size of display bar
        /// </summary>
        /// <param name="current">Current value to display</param>
        /// <param name="max">Max value to display</param>
        public void Update(float current, float max)
        {
            percentage = current / max;
        }

        /// <summary>
        /// Draws display bar on screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shade, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(solid, new Rectangle((int)position.X + border, (int)position.Y + border, (int)(((int)size.X - border*2)*percentage), (int)size.Y - border*2), null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
