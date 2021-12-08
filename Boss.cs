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

        private SoundEffectInstance bossAttack;

        public Boss(string name)
        {
            maxHealth = 2000;
            health = maxHealth;
            attackCooldown = 150;
            attackTime = 15;
            speed = 50f;
            IFrameCooldown = 3;
            this.name = name;

            scale = 2f;
            layerDepth = 0.4f;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("ready1_1");
            position = new Vector2(1000, 1000);

            for (int i = 0; i < 3; i++)
            {
                idleSprites.Add(content.Load<Texture2D>($"ready1_{i + 1}"));
            }

            for (int i = 0; i < 6; i++)
            {
                walkSprites.Add(content.Load<Texture2D>($"walk1_{i + 1}"));
            }

            for (int i = 0; i < 6; i++)
            {
                attackSprites.Add(content.Load<Texture2D>($"attack2_{i + 1}"));
            }

            bossAttack = content.Load<SoundEffect>("bossAttack").CreateInstance();
            bossAttack.Volume = 0.5f;

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPosition;
            MovementCheck();
            Move(gameTime);
            CheckIFrames();
            AttackCheck();
            collisionBox = new Rectangle((int)screenPosition.X - (int)origin.X, (int)screenPosition.Y - (int)origin.Y, sprite.Width, sprite.Height);
            Animate(gameTime);
        }

        public override void Attack()
        {
            bossAttack.Play();
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - new Vector2(25, 25) - Vector2.Normalize(screenPosition - playerTarget) * 25, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - new Vector2(25, 25) - Vector2.Normalize(screenPosition - playerTarget) * 75, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - new Vector2(25, 25) - Vector2.Normalize(screenPosition - playerTarget) * 125, 100, 25, 50, 50));
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
