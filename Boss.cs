using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;



namespace PixelSouls
{
    public class Boss : Enemy
    {
        public string name;

        private int attackCooldown;

        private SoundEffectInstance bossAttack;

        public Boss(string name)
        {
            maxHealth = 2000;
            health = maxHealth;
            attackCooldown = 0;
            speed = 50f;
            IFrameCooldown = 3;
            this.name = name;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("player");
            CreateOrigin();
            position = new Vector2(1000, 1000);

            bossAttack = content.Load<SoundEffect>("bossAttack").CreateInstance();
            bossAttack.Volume = 0.5f;

            for (int i = 0; i < 3; i++)
            {
                idleSprites.Add(content.Load<Texture2D>($"ready_{i + 1}"));
            }

            for (int i = 0; i < 6; i++)
            {
                walkSprites.Add(content.Load<Texture2D>($"walk1_{i + 1}"));
            }

            for (int i = 0; i < 6; i++)
            {
                attackSprites.Add(content.Load<Texture2D>($"attack2_{i + 1}"));
            }

            currentSpriteList = idleSprites;
        }

        public override void Update(GameTime gameTime)
        {
            velocity = GameWorld.player.Position - screenPosition;
            velocity.Normalize();
            //Move towards player
            screenPosition = position - GameWorld.CameraPosition;

            Move(gameTime);
            CheckIFrames();
            AttackCheck();
            collisionBox = new Rectangle((int)screenPosition.X - (int)origin.X, (int)screenPosition.Y - (int)origin.Y, sprite.Width, sprite.Height);

            Animate(gameTime);
        }

        private void AttackCheck()
        {
            if (attackCooldown == 0)
            {
                AttackRange();
                attackCooldown = 150;
            }
            if (attackCooldown > 0)
            {
                attackCooldown--;
            }
        }

        private void AttackRange()
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

            //Debug.WriteLine("An attack");
        }


        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
