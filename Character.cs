﻿using System;
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

        public void Rotate(Vector2 target, Vector2 self)
        {
            Vector2 target1 = new Vector2(target.X, target.Y);
            Vector2 Dpos = self - target1; //Vector between target and self

            rotation = (float)Math.Atan2(Dpos.Y, Dpos.X);
        }
    }
}
