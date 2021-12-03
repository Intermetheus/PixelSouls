using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;



namespace PixelSouls
{
    public class Boss : Enemy
    {
        private int attackCooldown;

        private SoundEffectInstance bossAttack;

        public Boss()
        {
            maxHealth = 2000;
            health = maxHealth;
            attackCooldown = 0;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("player");
            CreateOrigin();
            position = new Vector2(1000, 1000);

            bossAttack = content.Load<SoundEffect>("bossAttack").CreateInstance();
            bossAttack.Volume = 0.5f;
        }

        public override void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPosition;
            collisionBox = new Rectangle((int)screenPosition.X - (int)origin.X, (int)screenPosition.Y - (int)origin.Y, sprite.Width, sprite.Height);

            if (attackCooldown == 0)
            {
                attackRange();
                attackCooldown = 100;
            }
            if (attackCooldown > 0)
            {
                attackCooldown--;
            }
        }


        public void attackRange()
        {
            if (Vector2.Distance(screenPosition, GameWorld.player.Position) > 50)
            {
                Attack();
            }
            else if (collisionBox.Contains(GameWorld.player.Position.X, GameWorld.player.Position.Y))
            {
                Attack();
            }

        }

        public override void Attack()
        {
            bossAttack.Play();
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 25, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 75, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 125, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 175, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 225, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - GameWorld.player.Position) * 275, 100, 25, 50, 50));

            //Debug.WriteLine("An attack");
        }


        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
