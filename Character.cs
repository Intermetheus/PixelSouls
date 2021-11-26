using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    abstract class Character : GameObject
    {
        protected int health;
        protected int maxHealth;
        protected bool isAlive;

        protected int windupMS = 200;
        protected int attackDamage = 25;
        protected int attackWidth = 100;
        protected int attackHight = 50;

        public int HealthProp
        {
            get { return health; }
            set { health = value; }
        }


        public virtual void Attack()
        {

        }
        public virtual Rectangle AttackHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y, attackWidth, attackHight);
        }

        protected virtual void Move(GameTime gameTime)
        {
            float _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * _deltaTime);
        }

        public void Rotate(GameTime gameTime)
        {
            float _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Rotation code from twinstick?
        }
    }
}
