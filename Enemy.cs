using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Subclass of Character managing all elements of the character that are generic to all enemies.
    /// </summary>
    abstract public class Enemy : Character
    {
        protected int attackCooldown;   // Total attackCooldown
        protected int attackTime;   // The time when the boss starts attacking. Max = attackCooldown
        protected Vector2 playerTarget;
        protected int attackTrackingLag;   // Used to prevent an attacking enemy from tracking their target after starting an attack, mimmicing the lock-in mechanic on the player.
        protected int windupTimer;

        /// <summary>
        /// Runs the enemy's behaviour method. Updates screenPosition & collisionBox Rectangle
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        public override void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPositionProp;
            collisionBox = new Rectangle((int)screenPosition.X - (int)trueOrigin.X, (int)screenPosition.Y - (int)trueOrigin.Y, (int)trueOrigin.X * 2, (int)trueOrigin.Y * 2);
            
            Behaviour();
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the Characters position by adding the product of velocity, speed and deltaTime
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        protected override void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * deltaTime);
        }

        /// <summary>
        /// Governs enemy movement and attack behaviour. Currently the enemy has an attackCooldown and uses a set distance to determine when it can attack. The enemy moves directly towards the player.
        /// </summary>
        private void Behaviour()
        {
            if(attackCooldown <= 0)   // Is this enemy able to attack?
            {
                if (Vector2.Distance(screenPosition, GameWorld.PlayerProp.Position) < 250 && !isAttacking)   // Is the player close enough that this enemy wants to attack and not already attacking?
                {
                    isAttacking = true;

                    windupTimer = windup;

                    velocity = Vector2.Zero;

                    ChangeAnimationState(AnimState.Attack, attackSprites, origin, 5);

                    if (screenPosition.X < GameWorld.PlayerProp.Position.X)   // Determine what direction to face.
                    {
                        facingRight = true;
                        origin = new Vector2(40, 42);
                    }
                    else if (screenPosition.X > GameWorld.PlayerProp.Position.X)
                    {
                        facingRight = false;
                        origin = new Vector2(60, 42);
                    }

                    animationLock = true;
                }    

                if (isAttacking && windupTimer <= 0)   // Once the windup is complete, make an attack
                {
                    playerTarget = GameWorld.PlayerProp.TargetedPosition;
                    Attack();
                    GameWorld.PlayerProp.IsTargeted = false;
                    attackCooldown = 100;
                    isAttacking = false;
                    animationLock = false;
                }
                else if(isAttacking)   // Count down until windup is complete
                {
                    windupTimer--;

                    if (windupTimer <= attackTrackingLag)   // Cease tracking once tracking lag is matched.
                    {
                        GameWorld.PlayerProp.IsTargeted = true;
                    }
                }
            }
            else   // If attacking is on cooldown, reduce remaining cooldown
            {
                attackCooldown--;
            }

            if (!isAttacking)   // So long as the enemy is not attacking, use Move input to update position
            {
                velocity = GameWorld.PlayerProp.Position - screenPosition;
                velocity.Normalize();
            }           
        }
    }
}
