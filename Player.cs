using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelSouls
{
    /// <summary>
    /// Functionality relating to the player controlled character
    /// </summary>
    public class Player : Character
    {
        private MouseState mouseState;
        private KeyboardState keyState;

        private bool isDodge;
        private bool isTargeted;

        private bool iFrame;
        private int iFrameCooldown;
        private int iFrameCountDamage = 5;

        private int dodgeTime = 15;
        private int iFrameCountDodge = 15;
        private int dodgeLockTime = 20;
        private int dodgeStaminaCost;
        private int dodgeCooldown;
        private float dodgeSpeed;

        private int attackStaminaCost;
        private int stamina;
        private int maxStamina;

        private int healingTries;

        private int animationLockCooldown;
        private int lockTime;

        private Vector2 targetedPosition;   // Target position the boss attacks

        private float delay = 14;   // sound delay
        private float remainingDelay = 0;
        private SoundEffectInstance dodgeSound;
        private SoundEffectInstance walk1Sound;
        private SoundEffectInstance attackSound;

        /// <summary>
        /// Determines wether the player is targeted by the boss or not. Makes attacks dodgable
        /// </summary>
        public bool IsTargeted { set => isTargeted = value; }
        /// <summary>
        /// Reads current stamina to display on UI
        /// </summary>
        public int Stamina { get => stamina; }
        /// <summary>
        /// Reads max stamina to display on UI
        /// </summary>
        public int MaxStamina { get => maxStamina; }
        /// <summary>
        /// Position the boss is targeting. Updates to player position when player is targeted
        /// </summary>
        public Vector2 TargetedPosition { get => targetedPosition; }

        /// <summary>
        /// Sets initial values for player character
        /// </summary>
        public Player()
        {
            maxHealth = 100;
            health = maxHealth;
            maxStamina = 100;
            stamina = maxStamina;

            speed = 400;

            healingTries = 3;

            dodgeStaminaCost = 50;
            attackStaminaCost = 15;
            dodgeCooldown = 0;

            animationLock = false;
            animationLockCooldown = 0;

            scale = 2f;
            layerDepth = 0.4f;

            isAttacking = false;
        }

        /// <summary>
        /// Loads player related assets
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("ready_1");
            position = new Vector2(GameWorld.ScreenSizeProp.X / 2, GameWorld.ScreenSizeProp.Y / 2);

            for (int i = 0; i < 3; i++)
            {
                idleSprites.Add(content.Load<Texture2D>($"ready_{i + 1}"));
            }

            for (int i = 0; i < 6; i++)
            {
                walkSprites.Add(content.Load<Texture2D>($"walk_{i + 1}"));
            }

            for (int i = 0; i < 6; i++)
            {
                attackSprites.Add(content.Load<Texture2D>($"attack4_{i + 1}"));
            }

            dodgeSound = content.Load<SoundEffect>("dodge").CreateInstance();
            walk1Sound = content.Load<SoundEffect>("walk1").CreateInstance();
            attackSound = content.Load<SoundEffect>("attack").CreateInstance();
            damageSound = content.Load<SoundEffect>("damageSound").CreateInstance();

            walk1Sound.Volume = 0.5f;
            attackSound.Volume = 0.3f;

            base.LoadContent(content);
        }

        /// <summary>
        /// Runs the players different update checks. The player input is run from within the AnimationLock() method.
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        public override void Update(GameTime gameTime)
        {
            collisionBox = new Rectangle((int)position.X - (int)trueOrigin.X, (int)position.Y - (int)trueOrigin.Y, (int)trueOrigin.X * 2, (int)trueOrigin.Y * 2);
            AnimationLock();   // animationLock & Dodge
            CheckIFrames();

            //Aim();
            AttackCheck();
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws player sprite on screen, facing left or right
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference for drawing sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (facingRight)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, scale, SpriteEffects.None, layerDepth);
            }
            else
            {
                spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, scale, SpriteEffects.FlipHorizontally, layerDepth);
            }

        }

        /// <summary>
        /// HandleInput gets the mouseState and keyState. Checks which inputs the player is pressing.
        /// </summary>
        private void HandleInput()
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            velocity = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
                playWalkSound();
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(0, 1);
                playWalkSound();
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
                playWalkSound();
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
                playWalkSound();
            }

            if (keyState.IsKeyDown(Keys.F))
            {
                Heal();
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                Dodge();
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Attack();
            }
            void playWalkSound()
            {
                if (walk1Sound.State == SoundState.Stopped)
                {
                    remainingDelay = delay;
                    walk1Sound.Play();
                }
            }
        }

        // Currently unused, would be used for governing sprite orientation
        //private void Aim()
        //{
        //    Rotate(position, new Vector2(mouseState.X, mouseState.Y));
        //}

        /// <summary>
        /// Creates a rectangle in the future position of the player, and checks if that rectangle is inside the world and doesn't collide with another object.
        /// <para>If no collisions are found, the players position will be updated</para> 
        /// <para>Updates the position of the playerTarget, which is where the enemies can see the player. When the enemies are attacking the target will freeze position.</para> 
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        protected override void Move(GameTime gameTime)
        {
            // If the window is resized, the player will remain in the middle of the screen.
            // BUG: This can change the player, therefore moving outside the game area :(
            position = new Vector2(GameWorld.ScreenSizeProp.X / 2, GameWorld.ScreenSizeProp.Y / 2);

            // Save position from before move.
            initialPosition = GameWorld.CameraPositionProp;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool isColliding = false;

            // Create future player position(collision with objects)
            // Unused. However, it can be used for collision with obstacles in the future.
            int newX = (int)(position.X - origin.X + velocity.X * speed * deltaTime);
            int newY = (int)(position.Y - origin.Y + velocity.Y * speed * deltaTime);

            // Future Camera position (collision with worldSize)
            int cameraX = (int)(GameWorld.CameraPositionProp.X - origin.X + velocity.X * speed * dodgeSpeed * deltaTime);
            int cameraY = (int)(GameWorld.CameraPositionProp.Y - origin.Y + velocity.Y * speed * dodgeSpeed * deltaTime);

            // Future player collision
            Rectangle futurePosition = new Rectangle(newX, newY, sprite.Width, sprite.Height);   // Unused?
            Rectangle futureCamera = new Rectangle(cameraX, cameraY, sprite.Width, sprite.Height);

            // For collision with worldSize use future camera position
            // For collision with objects use futurePosition
            // This is because the futurePosition uses the player position, which is always in the middle of the screen.
            if (!Stage.WorldSize.Contains(futureCamera))
            {
                isColliding = true;
            }

            if (isColliding)
            {
                GameWorld.CameraPositionProp = initialPosition;
            }
            else
            {
                if (isDodge)
                {
                    float dodgeMultiplier = dodgeSpeed;
                    GameWorld.CameraPositionProp += velocity * speed * dodgeMultiplier * deltaTime;
                }
                GameWorld.CameraPositionProp += velocity * speed * deltaTime;
            }

            if (!isTargeted)
            {
                targetedPosition = position;
            }
            //If the player is under attack by an enemy, the target will move en the opposite direction of the player, therefore freezing in position.
            if (isTargeted)
            {
                targetedPosition -= velocity * speed * deltaTime;
            }
        }
        /// <summary>
        /// If the player has enough stamina and is not dodging already, a dodge is excecuted. The players dodge multiplier is increased, therefore moving the player faster.
        /// </summary>
        private void Dodge()
        {
            // dodgeCost is the amount of stamina used to dodge
            // BUG: if you dodge in opposite directions, you don't use stamina
            if (Stamina > dodgeStaminaCost && !isDodge)
            {
                if (keyState.IsKeyDown(Keys.W))
                {
                    velocity += new Vector2(0, -1);
                    dodgePreset();
                }
                if (keyState.IsKeyDown(Keys.S))
                {
                    velocity += new Vector2(0, 1);
                    dodgePreset();
                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    velocity += new Vector2(-1, 0);
                    dodgePreset();
                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    velocity += new Vector2(1, 0);
                    dodgePreset();
                }

                void dodgePreset()
                {
                    // Prevents the player from dodging twice(losing twice stamina) when moving diagonally.
                    if (!isDodge)
                    {
                        stamina -= dodgeStaminaCost;
                        dodgeSpeed = 1.5f;   // multiplier used in Move()
                        lockTime = dodgeLockTime;   // Time the player is animation locked for
                        iFrameCooldown = iFrameCountDodge;
                        animationLock = true;
                        isDodge = true;
                        dodgeSound.Play();
                    }
                }
            }
        }

        /// <summary>
        /// This method gives the player stamina each time Update() is called (if your stamina is below maxStamina)
        /// <para>It also handles cooldowns</para>
        /// <para>The higher animationLockCooldown is, the longer you can't input.</para>
        /// <para>The higher dodgeCooldown is the longer you dodge</para>
        /// </summary>
        private void AnimationLock()
        {
            if (Stamina < MaxStamina && !animationLock)   // Pauses stamina regeneration while taking an action
            {
                stamina++;

                if (Stamina > MaxStamina)
                {
                    stamina = MaxStamina;
                }
            }

            if (!animationLock)   // Player movement is only enabled when not animation locked
            {
                HandleInput();
            }
            else   // While locked cooldowns are advanced
            {
                animationLockCooldown++;
                dodgeCooldown++;

                if (dodgeCooldown >= dodgeTime && isDodge)   // Ends dodge when timer runs out
                {
                    isDodge = false;
                    velocity = velocity * 0.2f;
                }

                if (animationLockCooldown >= lockTime + dodgeTime)   // Ends animation lock when timer runs out
                {
                    dodgeCooldown = 0;
                    animationLockCooldown = 0;
                    animationLock = false;
                }
            }

            // Stop walkSound when movement keys are released
            if (velocity == Vector2.Zero)
            {
                remainingDelay -= 1;
                if (remainingDelay <= 0)
                {
                    walk1Sound.Stop();
                }
            }
        }

        /// <summary>
        /// Starts the player attack by setting a windup time and locking the player animation and velocity to zero.
        /// <para>Updates the player direction to the attacking direction</para>
        /// </summary>
        protected override void Attack()
        {
            if (!animationLock)
            {
                animationLock = true;
                isAttacking = true;
                windup = 15;
                lockTime = windup;

                velocity = Vector2.Zero;
                ChangeAnimationState(AnimState.Attack, attackSprites, origin, 10);

                if (mouseState.Position.X <= position.X)   // Ensures the sprite is facing the attack direction while attacking
                {
                    facingRight = false;
                    origin = new Vector2(60, 42);
                }
                else if (mouseState.Position.X > position.X)
                {
                    facingRight = true;
                    origin = new Vector2(40, 42);
                }
            }
        }

        /// <summary>
        /// Instantiates attackHitboxes when the windup time is over.
        /// <para>Consumes stamina</para>
        /// <para>Creates a new animation lock for a short time after the attack is over</para>
        /// <para>Plays the attack sound</para>
        /// </summary>
        private void AttackCheck()
        {
            if (isAttacking == true && windup <= 0)
            {
                GameWorld.Instantiate(new AttackHitbox(this, position - trueOrigin - Vector2.Normalize(position - new Vector2(mouseState.X, mouseState.Y)) * 25, 50, 50, 50));
                GameWorld.Instantiate(new AttackHitbox(this, position - trueOrigin - Vector2.Normalize(position - new Vector2(mouseState.X, mouseState.Y)) * 75, 50, 50, 50));

                stamina -= attackStaminaCost;

                animationLock = true;
                lockTime = 20;   // AnimationLock time
                isAttacking = false;
                attackSound.Play();
            }
            else if (isAttacking)
            {
                windup--;
            }
        }
        /// <summary>
        /// Decreases IFrameCooldown until it will set IFrame to false.
        /// </summary>
        private void CheckIFrames()
        {
            if (iFrameCooldown > 0)
            {
                iFrameCooldown--;
                iFrame = true;
            }
            else
            {
                iFrame = false;
            }
        }

        /// <summary>
        /// <para>Player can only move when not animation locked</para>
        /// Heals the player and decreases healingTries
        /// <para>animationLocks the player</para>
        /// </summary>
        private void Heal()
        {
            if (!animationLock && healingTries > 0 && health < maxHealth)
            {
                velocity = Vector2.Zero;
                ChangeAnimationState(AnimState.Idle, idleSprites, trueOrigin, 5);

                animationLock = true;
                lockTime = 100;

                health += 40;
                healingTries -= 1;

                if (health > maxHealth)
                {
                    health = maxHealth;
                }
            }
        }

        /// <summary>
        /// Checks for possible iframes before dealing damage to player
        /// </summary>
        /// <param name="attackDamage">Amount of damage to be dealt</param>
        public override void TakeDamage(int attackDamage)
        {
            if (!iFrame)
            {
                iFrame = true;
                iFrameCooldown = iFrameCountDamage;
                base.TakeDamage(attackDamage);
            }
        }

        /// <summary>
        /// Checkes if player is dead and changes gamestate accordingly
        /// </summary>
        protected override void CheckDeath()
        {
            if (health <= 0)
            {
                GameWorld.WinLoseStateProp = GameState.Lose;
            }
        }

        /// <summary>
        /// currently unused collision function
        /// </summary>
        /// <param name="other">gameobject player is colliding with</param>
        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
