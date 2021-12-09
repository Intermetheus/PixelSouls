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

        private SoundEffectInstance bossAttackSound;

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

            windup = 50;
            attackTrackingLag = 15;
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

            bossAttackSound = content.Load<SoundEffect>("bossAttack").CreateInstance();
            bossAttackSound.Volume = 0.5f;

            damageSound = content.Load<SoundEffect>("damageSound2").CreateInstance();

            base.LoadContent(content);
        }

        public override void Attack()
        {
            bossAttackSound.Play();
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - trueOrigin - Vector2.Normalize(screenPosition - playerTarget) * 25, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - trueOrigin - Vector2.Normalize(screenPosition - playerTarget) * 75, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - trueOrigin - Vector2.Normalize(screenPosition - playerTarget) * 125, 100, 25, 50, 50));
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
