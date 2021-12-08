using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    abstract public class Enemy : Character
    {

        protected int attackCooldown; //Total attackCooldown
        protected int attackTime; //The time when the boss starts attacking. Max = attackCooldown
        protected Vector2 playerTarget;

        public void MovementCheck()
        {
            if (attackCooldown > attackTime + 25) //Additive value determines the time the boss halts movement before an attack.
            {
                velocity = GameWorld.player.Position - screenPosition;
                velocity.Normalize();
            }
            else
            {
                velocity = Vector2.Zero;

                if (animState != AnimState.Attack)
                {
                    animState = AnimState.Attack;
                    currentSpriteList = attackSprites;
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
        }

        public void AttackCheck()
        {
            if (attackCooldown >= attackTime - 3 && attackCooldown < attackTime)
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
                animationLock = false;
            }
            if (attackCooldown > 0)
            {
                attackCooldown--;
            }
        }
    }
}
