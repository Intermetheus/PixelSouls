using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    abstract public class Enemy : Character
    {
        //public float timer;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //if(timer > 360)
            //{
            //    timer = 0;
            //}
            //else
            //{
            //    timer++;
            //}
            //rotation = timer;
        }

        public void FacePlayer()
        {

        }
    }
}
