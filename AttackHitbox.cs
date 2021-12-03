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
        private int windupMS;
        private int attackDamage;
        private int attackWidth;
        private int attackHight;

        private GameObject attacker;

        //private Rectangle collisionBox = new Rectangle();

        public override Rectangle CollisionBoxProp
        {
            get { return new Rectangle(collisionBox.X, collisionBox.Y, attackWidth, attackHight); }
            set { collisionBox = value; }
        }

        public AttackHitbox(GameObject self, Vector2 position, int windupMS, int attackDamage, int attackWidth, int attackHight)
        {
            this.attacker = self;
            this.windupMS = windupMS;
            this.attackDamage = attackDamage;
            this.attackWidth = attackWidth;
            this.attackHight = attackHight;

            collisionBox.X = (int)position.X;
            collisionBox.Y = (int)position.Y;
            collisionBox.Width = attackWidth;
            collisionBox.Height = attackHight;
        }

        public override void LoadContent(ContentManager content)
        {
            // Empty to allow the hitbox to be a game object, though it has no content of its own
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
                Debug.WriteLine(attackDamage);
                other.TakeDamage(attackDamage);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            GameWorld.Destroy(this);
        }


        //private void DealDamage(Character target)
        //{
        //    target.HealthProp -= attackDamage;
        //}
    }
}
