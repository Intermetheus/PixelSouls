using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PixelSouls
{
    public abstract class Character : GameObject
    {
        protected int health;
        protected int maxHealth;
        protected bool isAlive;
        protected int windup;
        protected bool IFrame;
        protected int IFrameCooldown;
      
        protected Vector2 initialPosition;       

        protected int windupMS = 200;
        protected int attackDamage = 1;
        protected int attackWidth = 100;
        protected int attackHeight = 50;

        public int HealthProp
        {
            get { return health; }
            set { health = value; }
        }
        public int MaxHealthProp
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }


        public virtual void Attack()
        {

        }
        public virtual Rectangle AttackHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y, attackWidth, attackHeight);
        }

        protected virtual void Move(GameTime gameTime)
        {
            float _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * _deltaTime);
        }

        public void Rotate(Vector2 self, Vector2 target)
        {
            Vector2 Dpos = target - self; //Vector between target and self

            rotation = (float)Math.Atan2(Dpos.Y, Dpos.X);
        }

        //public void CreateOrigin()
        //{
        //    origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        //}

        public override void TakeDamage(int attackDamage)
        {
            if (IFrame)
            {
            }
            else
            {
                health -= attackDamage;
                IFrame = true;
                CheckDeath();
            }
        }

        public void CheckDeath()
        {
            if(health <= 0)
            {
                if(GetType() == typeof(Player))
                {
                    GameWorld.WinLoseState = GameState.Lose;
                }
                else
                {
                    GameWorld.WinLoseState = GameState.Win;
                }
            }
        }
    }
}
