using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    class UI
    {
        protected Texture2D sprite;
        protected Vector2 position;

        public virtual void LoadContent(ContentManager content)
        {
            //sprite = content.Load<Texture2D>("");
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(sprite, position, Color.White);
        }
    }
}
