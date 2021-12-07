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
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected float speed;
        //protected float fps;
        protected float rotation;


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
            spriteBatch.Draw(sprite, screenPosition, null, Color.White, rotation, origin, 1F, SpriteEffects.None, 0.4f);
        }
        public abstract void OnCollision(GameObject other);

        public void CheckCollision(GameObject other)
        {
            if (CollisionBox().Intersects(other.CollisionBox()))
            {
                OnCollision(other);
            }
        }

        public virtual Rectangle CollisionBox()
        {
            if (this is Floor)
            {
                //No collisionBox
                return new Rectangle(20,20, 0, 0);
            }
            else
            { 
                return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            }
        }

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

        public void CreateOrigin()
        {
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }
    }
}
