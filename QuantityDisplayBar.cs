using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    abstract class QuantityDisplayBar
    {
        protected int border = 2;
        protected float percentage;
        protected Vector2 position;
        protected Vector2 size;
        protected Color color;
        protected Texture2D shade;
        protected Texture2D solid;


        public QuantityDisplayBar(ContentManager content, Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;

            shade = content.Load<Texture2D>("shade");
            solid = content.Load<Texture2D>("solid");
        }

        public virtual void Update(float current, float max)
        {
            percentage = current / max;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shade, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.Black);
            spriteBatch.Draw(solid, new Rectangle((int)position.X + border, (int)position.Y + border, (int)(((int)size.X - border*2)*percentage), (int)size.Y - border*2), color);
        }


    }
}
