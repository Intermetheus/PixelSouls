﻿using System;
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
    public class Player : Character
    {
        private MouseState mouseState;
        private KeyboardState keyState;

        private bool isDodge;
        private bool isAttacking;
        private bool isTargeted;
        private bool isHealing;

        private int dodgeCost;
        private int dodgeCooldown;
        private float dodgeSpeed;

        private int stamina;
        private int maxStamina;

        private int healingTries;

        private int animationLockCooldown;
        private int lockTime;

        private Vector2 targetedPosition; //Target position the boss attacks

        private SoundEffectInstance dodgeSound;
        private float delay = 14; //sound delay
        private float remainingDelay = 0;
        private SoundEffectInstance walk1Sound;
        private SoundEffectInstance walk2Sound;
        private SoundEffectInstance attackSound;
        private SoundEffectInstance damageSound;

        public bool IsTargeted { get => isTargeted; set => isTargeted = value; }
        public int Stamina { get => stamina; set => stamina = value; }
        public int MaxStamina { get => maxStamina; set => maxStamina = value; }
        public Vector2 TargetedPosition { get => targetedPosition; set => targetedPosition = value; }

        public Player()
        {
            maxHealth = 100;
            health = maxHealth;
            MaxStamina = 100;
            Stamina = maxStamina;

            speed = 400;

            healingTries = 3;

            dodgeCost = 50;
            dodgeCooldown = 0;
            dodgeSpeed = 10f; // multiplier

            IFrame = false;
            IFrameCooldown = 3;

            animationLock = false;
            animationLockCooldown = 0;

            scale = 2f;
            layerDepth = 0.4f;

            isAttacking = false;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("ready_1");
            position = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2);

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
            walk2Sound = content.Load<SoundEffect>("walk2").CreateInstance();
            attackSound = content.Load<SoundEffect>("attack").CreateInstance();
            damageSound = content.Load<SoundEffect>("damageSound").CreateInstance();

            walk1Sound.Volume = 0.5f;
            attackSound.Volume = 0.3f;

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            AnimationLock(); // animationLock & Dodge
            AttackCheck();
            Aim();
            Move(gameTime);
            CheckIFrames();
            collisionBox = new Rectangle((int)position.X - 25, (int)position.Y - 25, 50, 50);
            Animate(gameTime);
        }

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
                Healing();
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

        private void Aim()
        {
            Rotate(position, new Vector2(mouseState.X, mouseState.Y));
        }

        protected override void Move(GameTime gameTime)
        {
            //If the window is resized, the player will remain in the middle of the screen.
            //BUG: This can change the player, therefore moving outside the game area :(
            position = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2);

            //Save position from before move.
            initialPosition = GameWorld.CameraPosition;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool isColliding = false;

            //Create future player position(collision with objects)
            // Unused?
            int newX = (int)(position.X - origin.X + velocity.X * speed * deltaTime);
            int newY = (int)(position.Y - origin.Y + velocity.Y * speed * deltaTime);

            //Future Camera position (collision with worldSize)
            int cameraX = (int)(GameWorld.CameraPosition.X - origin.X + velocity.X * speed * dodgeSpeed * deltaTime);
            int cameraY = (int)(GameWorld.CameraPosition.Y - origin.Y + velocity.Y * speed * dodgeSpeed * deltaTime);

            //Future player collision
            Rectangle futurePosition = new Rectangle(newX, newY, sprite.Width, sprite.Height); // Unused?
            Rectangle futureCamera = new Rectangle(cameraX, cameraY, sprite.Width, sprite.Height);

            //For collision with worldSize use future camera position
            //This is because the futurePosition uses the player position, which is always in the middle of the screen.
            if (!Stage.WorldSize.Contains(futureCamera))
            {
                isColliding = true;
            }
            //For collision with objects use futurePosition

            if (isColliding)
            {
                GameWorld.CameraPosition = initialPosition;
            }
            else
            {
                if (isDodge)
                {
                    float dodgeMultiplier = dodgeSpeed;
                    GameWorld.CameraPosition += velocity * speed * dodgeMultiplier * deltaTime;
                }
                GameWorld.CameraPosition += velocity * speed * deltaTime;
            }
            if (!isTargeted)
            {
                targetedPosition = position;
            }
            if (isTargeted)
            {
                targetedPosition -= velocity * speed * deltaTime;
            }
        }

        private void Dodge()
        {
            //dodgeCost is the amount of stamina used to dodge
            //BUG: if you dodge in opposite directions, you don't use stamina
            if (Stamina > dodgeCost && !isDodge)
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
                    //Prevents the player from dodging twice(losing twice stamina) when moving diagonally.
                    if (!isDodge)
                    {
                        Stamina -= dodgeCost;
                        dodgeSpeed = 10f; //multiplier used in Move()
                        lockTime = 15; //Time the player is animation locked for
                        animationLock = true;
                        isDodge = true;
                        dodgeSound.Play();
                    }
                }
                //Dodge in mouse direction
                //Vector2 muse = new Vector2(mouseState.X, mouseState.Y);
                //Vector2 Dpos = muse - position;

                //velocity = Dpos;
            }
        }

        /// <summary>
        /// This method gives the player stamina each time Update() is called (if your stamina is below maxStamina)
        /// It also handles cooldowns
        /// The higher animationLockCooldown is, the longer you can't input.
        /// The higher dodgeCooldown is the longer you dodge
        /// </summary>
        private void AnimationLock()
        {
            if (Stamina < MaxStamina && !animationLock)
            {
                Stamina++;
                if (Stamina > MaxStamina)
                {
                    Stamina = MaxStamina;
                }
            }

            if (!animationLock)
            {
                HandleInput();
            }
            else
            {
                animationLockCooldown++;
                dodgeCooldown++;
                if (dodgeCooldown >= 3)
                {
                    dodgeCooldown = 0;
                    isDodge = false;
                }
                if (animationLockCooldown >= lockTime)
                {
                    animationLockCooldown = 0;
                    animationLock = false;
                }
            }
            //Stop walkSound when movement keys are released
            if (velocity == Vector2.Zero)
            {
                remainingDelay -= 1;
                if (remainingDelay <= 0)
                {
                    walk1Sound.Stop();
                }
            }

        }

        public override void Attack()
        {
            if (!animationLock)
            {
                animationLock = true;
                isAttacking = true;
                windup = 15;
                lockTime = windup;
                velocity = Vector2.Zero;
                animState = AnimState.Attack;
                currentSpriteList = attackSprites;
                if (mouseState.Position.X <= position.X)
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
            //Debug.WriteLine("An attack");
        }

        private void AttackCheck()
        {
            if (isAttacking && windup <= 0)
            {
                GameWorld.Instantiate(new AttackHitbox(this, position - new Vector2(25, 25) - Vector2.Normalize(position - new Vector2(mouseState.X, mouseState.Y)) * 25, 100, 50, 50, 50));
                GameWorld.Instantiate(new AttackHitbox(this, position - new Vector2(25, 25) - Vector2.Normalize(position - new Vector2(mouseState.X, mouseState.Y)) * 75, 100, 50, 50, 50));

                animationLock = true;
                lockTime = 20; //AnimationLock time
                isAttacking = false;
                attackSound.Play();
            }
            else if (isAttacking)
            {
                windup--;
            }
        }

        protected override void CheckIFrames()
        {
            if (isDodge && IFrameCooldown >= 0)
            {
                IFrame = true;
            }
            else
            {
                IFrame = false;
            }
            if (IFrame && IFrameCooldown >= 0)
            {
                IFrameCooldown--;
            }
            else
            {
                IFrameCooldown = 3;
                IFrame = false;
            }
            //Add other iFrame checks here
        }
        
        /// <summary>
        /// Override  so we can play the damageSound
        /// </summary>
        /// <param name="attackDamage"></param>
        public override void TakeDamage(int attackDamage)
        {
            if (IFrame)
            {
            }
            else
            {
                damageSound.Play();
                health -= attackDamage;
                IFrame = true;
                CheckDeath();
            }
        }

        private void Healing()
        {
            if (!animationLock)
            {
                if (this.healingTries > 0)
                {
                    if (health < 100)
                    {
                        isHealing = true;
                        animationLock = true;
                        windup = 10;
                        this.health += 40;
                        this.healingTries -= 1;
                        if (health > 100)
                        {
                            this.health = 100;
                        }
                    }
                }
            }
        }

        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
