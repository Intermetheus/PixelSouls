using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PixelSouls
{
    public enum GameState { Play, Menu, Lose, Win }

    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static SpriteFont arial;

        private Texture2D collisionTexture;

        public static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static Vector2 screenSize;
        private static Vector2 cameraPosition = new Vector2(800, 800);

        public static Player player = new Player();
        public static Boss boss = new Boss("Onesiphorus The Blasphemed");

        private static GameState winLoseState;

        private SoundEffectInstance backgroundMusic;

        public static GameState WinLoseState
        {
            get { return winLoseState; }
            set { winLoseState = value; }
        }

        public static Vector2 CameraPosition { get => cameraPosition; set => cameraPosition = value; }
        public static Vector2 ScreenSize { get => screenSize; set => screenSize = value; }

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        protected override void Initialize()
        {
            winLoseState = GameState.Play;
            gameObjects.Add(player);
            gameObjects.Add(boss);
            gameObjects.Add(new Target());

            base.Initialize();
            Stage.LoadLevel("PrototypePlayground");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>("Arial");

            backgroundMusic = Content.Load<SoundEffect>("music").CreateInstance();
            backgroundMusic.Volume = 0.1f;
            backgroundMusic.Play();
            backgroundMusic.IsLooped = true;

            collisionTexture = Content.Load<Texture2D>("collisionTexture");

            //Sprites used in the stage have to be loaded here
            Stage.LoadContent(Content);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }

            UI.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(winLoseState == GameState.Play)
            {
                gameObjects.AddRange(newGameObjects);
                newGameObjects.Clear();

                foreach (GameObject gameObject in removeGameObjects)
                {
                    gameObjects.Remove(gameObject);
                }
                removeGameObjects.Clear();

                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(gameTime);

                    foreach (GameObject other in gameObjects)
                    {
                        if (gameObject != other)
                        {
                            if (gameObject.IsColliding(other))
                            {
                                gameObject.OnCollision(other);
                                other.OnCollision(gameObject);
                            }
                        }
                    }
                }
                UI.Update(gameTime);
            }            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);   // Makes layers work           

            if (winLoseState == GameState.Play)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Draw(spriteBatch);
#if DEBUG
                    DrawCollisionBox(gameObject.CollisionBoxProp);
#endif
                }
                UI.Draw(spriteBatch);

                DrawWorldBoundary(Stage.WorldSize);
            }
            else if (winLoseState == GameState.Win)
            {
                spriteBatch.DrawString(arial, "You Win", new Vector2(screenSize.X / 2 - 100, screenSize.Y / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }
            else if (winLoseState == GameState.Lose)
            {
                spriteBatch.DrawString(arial, "You Suck", new Vector2(screenSize.X / 2 - 100, screenSize.Y / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        public static void Destroy(GameObject gameObject)
        {
            removeGameObjects.Add(gameObject);
        }

        // This is used to draw the Stages walls
        private void DrawCollisionBox(Rectangle rect)
        {
            DrawLines(rect, Color.Red, 1);
        }

        private void DrawWorldBoundary(Rectangle rect)
        {
            Rectangle collisionBox = rect;

            collisionBox.X = collisionBox.X - (int)CameraPosition.X + (int)ScreenSize.X / 2;
            collisionBox.Y = collisionBox.Y - (int)CameraPosition.Y + (int)ScreenSize.Y / 2;

            DrawLines(collisionBox, Color.DarkGray, 10);
        }

        private void DrawLines(Rectangle rect, Color color, int lineWidth)
        {
            Rectangle topLine = new Rectangle(rect.X, rect.Y, rect.Width, lineWidth);
            Rectangle bottomLine = new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, lineWidth);
            Rectangle rightLine = new Rectangle(rect.X + rect.Width, rect.Y, lineWidth, rect.Height + lineWidth);
            Rectangle leftLine = new Rectangle(rect.X, rect.Y, lineWidth, rect.Height);

            spriteBatch.Draw(collisionTexture, topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(collisionTexture, bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(collisionTexture, rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(collisionTexture, leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
    }
}
