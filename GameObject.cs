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
        protected Vector2 origin;

        protected float speed;
        protected float rotation;

        protected Texture2D sprite;
        protected Texture2D[] sprites;

        protected float scale;
        protected float layerDepth;


        protected Rectangle collisionBox;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual Rectangle CollisionBoxProp
        {
            get { return collisionBox; }
            set { collisionBox = value; }
        }

        public abstract void LoadContent(ContentManager content);

        public virtual void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPosition;
            if (this is Floor)
            {

            }
            else
            {
                collisionBox = new Rectangle((int)screenPosition.X - (int)origin.X, (int)screenPosition.Y - (int)origin.Y, sprite.Width, sprite.Height);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, screenPosition, null, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }
        public abstract void OnCollision(GameObject other);       

        public virtual void TakeDamage(int damage)
        {
            throw new NotImplementedException("Cannot deal damage to non-character objects");
        }

        public bool IsColliding(GameObject other)
        {
            if (CollisionBoxProp.Intersects(other.CollisionBoxProp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void CreateOrigin()
        {
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }
    }
}
