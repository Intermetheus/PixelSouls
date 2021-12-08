using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PixelSouls
{
    public enum AnimState { Idle, Walk, Attack}

    public abstract class Character : GameObject
    {
        protected int health;
        protected int maxHealth;
        protected bool isAlive;

        protected int windup;

        protected bool IFrame;
        protected int IFrameCooldown;
      
        protected Vector2 initialPosition;
        protected Vector2 trueOrigin;

        protected int windupMS = 200;
        protected int attackDamage = 1;
        protected int attackWidth = 100;
        protected int attackHeight = 50;

        protected AnimState animState;
        protected bool facingRight;
        protected float animCounter;
        protected int currentSpriteIndex;
        protected float fps = 5f;
        protected List<Texture2D> currentSpriteList = new List<Texture2D>();
        protected List<Texture2D> idleSprites = new List<Texture2D>();
        protected List<Texture2D> walkSprites = new List<Texture2D>();
        protected List<Texture2D> attackSprites = new List<Texture2D>();
        protected bool animationLock;

        public int HealthProp
        {
            get { return health; }
            set { health = value; }
        }
        public int MaxHealthProp
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public override void LoadContent(ContentManager content)
        {
            currentSpriteList = idleSprites;
            CreateOrigin();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (facingRight)
            {
                spriteBatch.Draw(sprite, screenPosition, null, Color.White, 0, origin, scale, SpriteEffects.None, layerDepth);
            }
            else
            {
                spriteBatch.Draw(sprite, screenPosition, null, Color.White, 0, origin, scale, SpriteEffects.FlipHorizontally, layerDepth);
            }
        }

        public abstract void Attack();

        protected virtual void CheckIFrames()
        {
            if (IFrame && IFrameCooldown >= 0)
            {
                IFrameCooldown--;
            }
            else
            {
                IFrameCooldown = 3;
                IFrame = false;
            }
        }

        protected virtual void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * deltaTime);
        }

        public void Rotate(Vector2 self, Vector2 target)
        {
            Vector2 Dpos = target - self; //Vector between target and self

            rotation = (float)Math.Atan2(Dpos.Y, Dpos.X);
        }

        public override void TakeDamage(int attackDamage)
        {
            if (IFrame)
            {
            }
            else
            {
                health -= attackDamage;
                IFrame = true;
                CheckDeath();
            }
        }

        public void CheckDeath()
        {
            if(health <= 0)
            {
                if(GetType() == typeof(Player))
                {
                    GameWorld.WinLoseState = GameState.Lose;
                }
                else
                {
                    GameWorld.WinLoseState = GameState.Win;
                }
            }
        }

        public override void CreateOrigin()
        {
            base.CreateOrigin();
            trueOrigin = origin;
        }

        public void Animate(GameTime gameTime)
        {
            if(velocity == Vector2.Zero && animState != AnimState.Idle && !animationLock)
            {
                ChangeAnimationState(AnimState.Idle, idleSprites, trueOrigin, 5);
            }
            else if(velocity != Vector2.Zero && animState != AnimState.Walk && !animationLock)
            {
                ChangeAnimationState(AnimState.Walk, walkSprites, trueOrigin, 5);
            }

            if(velocity.X > 0)
            {
                facingRight = true;
            }
            else if(velocity.X < 0)
            {
                facingRight = false;
            }

            animCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentSpriteIndex = (int)(animCounter * fps);          

            if (currentSpriteIndex > currentSpriteList.Count - 1)
            {
                ResetAnimation();
            }

            sprite = currentSpriteList[currentSpriteIndex];
        }

        public void ChangeAnimationState(AnimState animState, List<Texture2D> spriteList, Vector2 origin, float fps)
        {
            this.animState = animState;
            currentSpriteList = spriteList;
            ResetAnimation();
            this.origin = origin;
            this.fps = fps;
        }

        public void ResetAnimation()
        {
            animCounter = 0;
            currentSpriteIndex = 0;
        }
    }
}
