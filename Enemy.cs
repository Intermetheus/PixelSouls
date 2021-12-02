using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    abstract public class Enemy : Character
    {

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            collisionBox = new Rectangle((int)position.X - (int)origin.X, (int)position.Y - (int)origin.Y, sprite.Width, sprite.Height);
        }

        public override void Attack()
        {

        }

        protected override void Move(GameTime gameTime)
        {

        }

        public void FacePlayer()
        {

        }
    }
}
