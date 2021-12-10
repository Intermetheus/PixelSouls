using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    class QuantityDisplayBar
    {
        private int border = 2;
        private float percentage;
        private Vector2 position;
        private Vector2 size;
        private Color color;
        private Texture2D shade;
        private Texture2D solid;


        public QuantityDisplayBar(ContentManager content, Vector2 position, Color color, Vector2 size)
        {
            this.position = position;
            this.color = color;
            this.size = size;

            shade = content.Load<Texture2D>("shade");
            solid = content.Load<Texture2D>("solid");
        }

        public void Update(float current, float max)
        {
            percentage = current / max;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shade, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(solid, new Rectangle((int)position.X + border, (int)position.Y + border, (int)(((int)size.X - border*2)*percentage), (int)size.Y - border*2), null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
