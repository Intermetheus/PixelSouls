using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PixelSouls
{
    class AttackHitbox : GameObject
    {
        private int attackDamage;
        private int attackWidth;
        private int attackHeight;

        private GameObject attacker;

        public override Rectangle CollisionBoxProp
        {
            get { return new Rectangle(collisionBox.X, collisionBox.Y, attackWidth, attackHeight); }
            set { collisionBox = value; }
        }

        public AttackHitbox(GameObject attacker, Vector2 position, int attackDamage, int attackWidth, int attackHeight)
        {
            this.attacker = attacker;
            this.attackDamage = attackDamage;
            this.attackWidth = attackWidth;
            this.attackHeight = attackHeight;

            collisionBox.X = (int)position.X;
            collisionBox.Y = (int)position.Y;
            collisionBox.Width = attackWidth;
            collisionBox.Height = attackHeight;
        }

        public override void LoadContent(ContentManager content)
        {
            // Empty to allow the hitbox to be a game object, though it has no content of its own
        }

        public override void Update(GameTime gameTime)
        {
            GameWorld.Destroy(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Empty to allow the hitbox to be a game object, though it has no sprite of its own
        }

        /// <summary>
        /// OnCollision override for the attack hitbox, purely concerned with dealing damage to objects it intersects with.
        /// Looks to make sure the GameObject being hit is actually a character and that it is not the attacker themselves.
        /// If these conditions are met, the GameObject that was hit is instructed to run its TakeDamage method.
        /// </summary>
        /// <param name="other">The game object being hit by the attack</param>
        public override void OnCollision(GameObject other)
        {
            if (other is Character && attacker != other)
            {
                other.TakeDamage(attackDamage);
            }
        }
    }
}
