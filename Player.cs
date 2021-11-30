using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelSouls
{
    public class Player : Character
    {
        private int stamina;
        private int maxStamina;
        private bool animationLock;
        private int animationLockCooldown;
        private Vector2 origin;
        private MouseState mouseState;
        private KeyboardState keyState;

        public Player()
        {
            animationLock = false;
            animationLockCooldown = 0;
            speed = 400;
            origin = new Vector2(25,25); //Should be in the middle of the sprites texture
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Debug.WriteLine();
            spriteBatch.Draw(sprite, position, null, Color.White, rotation + 3.14f, origin, 1F, SpriteEffects.None, 0.2f);
        }

        public override void Update(GameTime gameTime)
        {
            if (!animationLock && animationLockCooldown < 2000)
            {
                HandleInput();
                animationLockCooldown++;
            }
            else
            {
                animationLock = false;
                animationLockCooldown = 0;
            }
            Aim();
            Move(gameTime);
        }

        private void HandleInput()
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            velocity = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(0, 1);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                Dodge();
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
        }
        private void LightAttack()
        {

        }

        private void HeavyAttack()
        {

        }
        protected override void Move(GameTime gameTime)
        {
            //If the window is resized, the player will remain in the middle of the screen.
            //BUG: This can change the player, therefore moving outside the game area :(
            position = new Vector2(GameWorld.ScreenSize.X / 2 - sprite.Width / 2, GameWorld.ScreenSize.Y / 2 - sprite.Height / 2);

            //Save position from before move.
            initialPosition = GameWorld.CameraPosition;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool isColliding = false;

            //Create future player position(collision with objects)
            int newX = (int)(position.X + velocity.X * speed * deltaTime);
            int newY = (int)(position.Y + velocity.Y * speed * deltaTime);

            //Future Camera position (collision with worldSize)
            int cameraX = (int)(GameWorld.CameraPosition.X + velocity.X * speed * deltaTime);
            int cameraY = (int)(GameWorld.CameraPosition.Y + velocity.Y * speed * deltaTime);

            //Future player collision
            Rectangle futurePosition = new Rectangle(newX, newY, sprite.Width, sprite.Height);
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
                GameWorld.CameraPosition += velocity * speed * deltaTime;
            }
        }

        private void Dodge()
        {
            animationLock = true;
            Vector2 muse = new Vector2(mouseState.X, mouseState.Y);
            Vector2 Dpos = muse - position;

            velocity = Dpos;
        }

        private void Aim()
        {
            Rotate(position, new Vector2(mouseState.X, mouseState.Y));
            //Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            //Vector2 Dpos = position - mousePosition; //Vector between player and mouse

            //rotation = (float)Math.Atan2(Dpos.Y, Dpos.X);
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("player");
            position = new Vector2(GameWorld.ScreenSize.X / 2-sprite.Width/2, GameWorld.ScreenSize.Y / 2-sprite.Height/2);
            //Stage.WorldSize = new Rectangle(0, 0, 5000, 5000); //TEMPORARY: Remove this code once Stage has been implemented
        }

        public override void OnCollision(GameObject other)
        {

        }

        
    }
}
