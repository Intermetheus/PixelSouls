using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PixelSouls
{
    /// <summary>
    /// Describes possible animation states.
    /// </summary>
    public enum AnimState { Idle, Walk, Attack }

    /// <summary>
    /// Character class managing all elements that are generic to all characters.
    /// </summary>
    public abstract class Character : GameObject
    {
        protected int health;
        protected int maxHealth;
        protected bool isAlive;

        protected bool isAttacking;

        protected bool hasTakenDamage;

        protected int windup;

        protected Vector2 initialPosition;
        protected Vector2 trueOrigin;   // A mirror of the origin that does not get updated as the sprite changes

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

        protected SoundEffectInstance damageSound;

        /// <summary>
        /// Used by the UI to display the health of a character
        /// </summary>
        public int HealthProp
        {
            get { return health; }
            set { health = value; }
        }
        /// <summary>
        /// Used by the UI to display the max health of a character
        /// </summary>
        public int MaxHealthProp
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// Sets currentSpriteList to idleSprites, because the character is idle at the start of the game.
        /// Creates the origin and true origin of the character.
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public override void LoadContent(ContentManager content)
        {
            currentSpriteList = idleSprites;
            CreateOrigin();
        }

        /// <summary>
        /// All characters must move and animate continously.
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
            hasTakenDamage = false;   // Used to ensure one attack does not inflict multiple instances of damage with different hitboxes.
        }

        /// <summary>
        /// Draws the character depending on the direction they are facing.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference for drawing sprites</param>
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

        /// <summary>
        /// Most characters will require an attack, however the player and non-player characters must attack using very different logic
        /// </summary>
        protected abstract void Attack();

        /// <summary>
        /// Most characters will require movement, however the player and non-player characters must move using very different logic
        /// </summary>
        protected abstract void Move(GameTime gameTime);


        // Eventually to be converted into a eight-directional sprite changer, based on the direction the character is facing
        // Currently unused
        
        //protected void Rotate(Vector2 self, Vector2 target)
        //{
        //    Vector2 Dpos = target - self; //Vector between target and self

        //    rotation = (float)Math.Atan2(Dpos.Y, Dpos.X);
        //}
        
        
        /// <summary>
        /// Removes health from the character, based on the value in the parameter and checks if this kills the character.
        /// </summary>
        /// <param name="attackDamage">The damage taken</param>
        public override void TakeDamage(int attackDamage)
        {
            if (!hasTakenDamage)   // Ensures one attack does not inflict damage multiple times if overlapping hitboxes touch the character
            {
                hasTakenDamage = true;
                damageSound.Play();
                health -= attackDamage;
                CheckDeath();
            }          
        }

        /// <summary>
        /// Checks if health is zero or below, then destroys the Character
        /// </summary>
        protected virtual void CheckDeath()
        {
            if (health <= 0)
            {
                GameWorld.Destroy(this);
            }
        }

        /// <summary>
        /// Runs the CreateOrigin on the base, GameObject, and sets trueOrigin to the origin value.
        /// 'True origin' is a mirror of the origin that does not get updated as the sprite changes.
        /// </summary>
        protected override void CreateOrigin()
        {
            base.CreateOrigin();
            trueOrigin = origin;
        }

        /// <summary>
        /// Looks at the game state of the character and ensures the correct animation is played.
        /// </summary>
        /// <param name="gameTime">Time reference for running animation code at a fixed interval</param>
        private void Animate(GameTime gameTime)
        {
            if(velocity == Vector2.Zero && animState != AnimState.Idle && !animationLock)   // Checks if moving.
            {
                ChangeAnimationState(AnimState.Idle, idleSprites, trueOrigin, 5);
            }
            else if(velocity != Vector2.Zero && animState != AnimState.Walk && !animationLock)
            {
                ChangeAnimationState(AnimState.Walk, walkSprites, trueOrigin, 5);
            }

            if(velocity.X > 0)   // Checks direction of movement.
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

        /// <summary>
        /// Generic method for changing animation stat that can more easily be reused and called elsewhere.
        /// </summary>
        /// <param name="animState">The animation stat being changed to</param>
        /// <param name="spriteList">The sprite list now in use</param>
        /// <param name="origin">Origin of the new sprite</param>
        /// <param name="fps">The playback speed of the new animation</param>
        protected void ChangeAnimationState(AnimState animState, List<Texture2D> spriteList, Vector2 origin, float fps)
        {
            this.animState = animState;
            currentSpriteList = spriteList;
            ResetAnimation();
            this.origin = origin;
            this.fps = fps;
        }

        private void ResetAnimation()
        {
            animCounter = 0;
            currentSpriteIndex = 0;
        }
    }
}
