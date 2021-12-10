using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    abstract public class Enemy : Character
    {
        protected int attackCooldown;   // Total attackCooldown
        protected int attackTime;   // The time when the boss starts attacking. Max = attackCooldown
        protected Vector2 playerTarget;
        protected int attackTrackingLag;
        protected int windupTimer;

        public override void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPosition;
            collisionBox = new Rectangle((int)screenPosition.X - (int)trueOrigin.X, (int)screenPosition.Y - (int)trueOrigin.Y, (int)trueOrigin.X * 2, (int)trueOrigin.Y * 2);
            
            Behaviour();
            base.Update(gameTime);
        }

        protected override void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * deltaTime);
        }

        /// <summary>
        /// Governs enemy movement and attack behaviour
        /// </summary>
        private void Behaviour()
        {
            if(attackCooldown <= 0)
            {
                if (Vector2.Distance(screenPosition, GameWorld.player.Position) < 250 && !isAttacking)
                {
                    isAttacking = true;

                    windupTimer = windup;

                    velocity = Vector2.Zero;

                    if (animState != AnimState.Attack)
                    {
                        ChangeAnimationState(AnimState.Attack, attackSprites, origin, 5);

                        if (screenPosition.X < GameWorld.player.Position.X)
                        {
                            facingRight = true;
                            origin = new Vector2(40, 42);
                        }
                        else if (screenPosition.X > GameWorld.player.Position.X)
                        {
                            facingRight = false;
                            origin = new Vector2(60, 42);
                        }
                        animationLock = true;
                    }
                }    

                if (isAttacking && windupTimer <= 0)
                {
                    playerTarget = GameWorld.player.TargetedPosition;
                    Attack();
                    GameWorld.player.IsTargeted = false;
                    attackCooldown = 100;
                    isAttacking = false;
                    animationLock = false;
                }
                else if(isAttacking)
                {
                    windupTimer--;

                    if (windupTimer <= attackTrackingLag)
                    {
                        GameWorld.player.IsTargeted = true;
                    }
                }
            }
            else
            {
                attackCooldown--;
            }

            if (!isAttacking)
            {
                velocity = GameWorld.player.Position - screenPosition;
                velocity.Normalize();
            }           
        }
    }
}
