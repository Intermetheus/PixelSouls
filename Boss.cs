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

        private int attackCooldown; //Total attackCooldown
        private int attackTime; //The time when the boss starts attacking. Max = attackCooldown
        private Vector2 playerTarget;

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
            MovementCheck();
            Move(gameTime);
            CheckIFrames();
            AttackCheck();
            collisionBox = new Rectangle((int)screenPosition.X - (int)origin.X, (int)screenPosition.Y - (int)origin.Y, sprite.Width, sprite.Height);


        }

        private void MovementCheck()
        {
            if (attackCooldown > attackTime + 25) //Additive value determines the time the boss halts movement before an attack.
            {
                velocity = GameWorld.player.Position - screenPosition;
                velocity.Normalize();
            }
            else
            {
                velocity = Vector2.Zero;
            }
        }

        private void AttackCheck()
        {
            if (attackCooldown >= attackTime-3 && attackCooldown < attackTime)
            {
                GameWorld.player.IsTargeted = true;
            }
            else
            {
            }
            if (attackCooldown == 0)
            {
                playerTarget = GameWorld.player.TargetedPosition;
                if (Vector2.Distance(screenPosition, GameWorld.player.Position) > 50)
                {
                    Attack();
                    GameWorld.player.IsTargeted = false;
                    attackCooldown = 150;
                }
                else if (collisionBox.Contains(GameWorld.player.Position.X, GameWorld.player.Position.Y))
                {
                    Attack();
                    GameWorld.player.IsTargeted = false;
                    attackCooldown = 150;
                }
            }
            if (attackCooldown > 0)
            {
                attackCooldown--;
            }
        }

        public override void Attack()
        {
            bossAttack.Play();
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - playerTarget) * 25, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - playerTarget) * 75, 100, 25, 50, 50));
            GameWorld.Instantiate(new AttackHitbox(this, screenPosition - origin - Vector2.Normalize(screenPosition - playerTarget) * 125, 100, 25, 50, 50));

            //Debug.WriteLine("An attack");
        }


        public override void OnCollision(GameObject other)
        {

        }
    }
}
