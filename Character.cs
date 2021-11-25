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

        public virtual void Attack()
        {

        }

        protected virtual void Move()
        {

        }

        public void Rotate()
        {

        }
    }
}
