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

        public override void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPosition;
            attackRange();
        }


        public void attackRange()
        {
            if (Vector2.Distance(screenPosition, GameWorld.player.Position) > 50)
            {
                Attack();
            }

        }

        public override void Attack()
        {
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 25, 100, 50, 50, 50));
            //Debug.WriteLine("An attack");
        }


        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
