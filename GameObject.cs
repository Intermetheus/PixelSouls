using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    public abstract class GameObject
    {
        protected Vector2 screenPosition;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected float speed;
        protected float fps;

        public abstract void LoadContent(ContentManager content);

        public virtual void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPosition;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public abstract void OnCollision(GameObject other);
        public abstract void OnCollision(Character character);

        public void CheckCollision(GameObject other)
        {
            if (CollisionBox().Intersects(other.CollisionBox()))
            {
                OnCollision(other);
            }
        }

        public virtual Rectangle CollisionBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }
    }
}
