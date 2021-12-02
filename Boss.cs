using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    public class Boss : Enemy
    {
        public Boss()
        {
            maxHealth = 2000;
            health = maxHealth;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("player");
            CreateOrigin();
            position = new Vector2(1000, 1000);
        }

        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
