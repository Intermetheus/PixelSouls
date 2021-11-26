using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PixelSouls
{
    public abstract class Character : GameObject
    {
        protected int health;
        protected int maxHealth;
        protected bool isAlive;
        protected Vector2 initialPosition;

        public virtual void Attack()
        {

        }

        protected virtual void Move(GameTime gameTime)
        {

        }

        public void Rotate()
        {

        }
    }
}
