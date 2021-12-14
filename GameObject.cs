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

        /// <summary>
        /// The position Property is used to get the position of other objects.
        /// <para>Currently used by the Enemy to see where the player is, in this prototype</para>
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// The collisionBox Property is useful to get interaction between objects, when they intersect
        /// </summary>
        public virtual Rectangle CollisionBoxProp
        {
            get { return collisionBox; }
        }

        /// <summary>
        /// LoadContent is an abstract, so that each subclass can override it with their own assets.
        /// <para>LoadContent will be called in the GameWorld on every GameObject</para>
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Updates the screenPosition of objects based on camera position and actual position in the gameWorld.
        /// <para>screenPosition variable is used to draw the sprites correctly, dependent on where the camera has moved.</para>
        /// <para>Updates the position of the collisionBox</para>
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        public virtual void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPositionProp;
            collisionBox = new Rectangle((int)screenPosition.X - (int)origin.X, (int)screenPosition.Y - (int)origin.Y, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Draws the sprite with the correct position, rotation, origin, scale, effect and layer
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference for drawing</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, screenPosition, null, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// OnCollision is currently used by attackHitBox, but can later on be used with collision between player and loot etc.
        /// </summary>
        /// <param name="other">other can be used to compare which class is collided with.
        /// <para>Example: if (other is Enemy){}</para></param>
        public abstract void OnCollision(GameObject other);       

        /// <summary>
        /// TakeDamage is used by characters to substract health based on the damage parameter
        /// </summary>
        /// <param name="damage">Damage is the amount that will be substracted from the health of Characters</param>
        public virtual void TakeDamage(int damage)
        {
            throw new NotImplementedException("Cannot deal damage to non-character objects");
        }

        /// <summary>
        /// IsColliding is used to call the OnCollision method
        /// <para>This method is called on every object from the Update() method in GameWorld</para>
        /// </summary>
        /// <param name="other">other is the object that has been collided with</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets value of origin to the center of the sprites texture.
        /// </summary>
        protected virtual void CreateOrigin()
        {
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }
    }
}
