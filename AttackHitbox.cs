using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    class AttackHitbox : GameObject
    {
        private int windupMS;
        private int attackDamage;
        private int attackWidth;
        private int attackHight;

        private Rectangle collisionBox = new Rectangle();

        public override Rectangle CollisionBoxProp
        {
            get { return new Rectangle((int)position.X, (int)position.Y, attackWidth, attackHight); }
            set { collisionBox = value; }
        }

        public AttackHitbox(Vector2 position, int windupMS, int attackDamage, int attackWidth, int attackHight)
        {
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

        public override void OnCollision(GameObject other)
        {
            if (other is Character)
            {
                other.TakeDamage(attackDamage);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GameWorld.Destroy(this);
        }


        //private void DealDamage(Character target)
        //{
        //    target.HealthProp -= attackDamage;
        //}
    }
}
