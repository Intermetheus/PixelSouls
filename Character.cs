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
        protected float rotation;

        public virtual void Attack()
        {

        }

        protected virtual void Move(GameTime gameTime)
        {

        }

        public void Rotate(Vector2 target, Vector2 self)
        {
            Vector2 target1 = new Vector2(target.X, target.Y);
            Vector2 Dpos = self - target1; //Vector between player and mouse

            rotation = (float)Math.Atan2(Dpos.Y, Dpos.X);
        }
    }
}
