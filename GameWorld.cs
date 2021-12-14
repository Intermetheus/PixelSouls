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
    /// <summary>
    /// Contains possible game states
    /// </summary>
    public enum GameState { Play, Menu, Lose, Win }

    /// <summary>
    /// Functionality relating to the game world
    /// </summary>
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static SpriteFont arial;

        private Texture2D collisionTexture;

        // Static fields allow the Instatiate and Destroy methods to be static
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static Vector2 screenSize;
        private static Vector2 cameraPosition = new Vector2(800, 800);

        private static Player player = new Player();
        private static Boss boss = new Boss("Onesiphorus The Blasphemed");

        private static GameState winLoseState;

        private SoundEffectInstance backgroundMusic;

        /// <summary>
        /// Font for writing text on screen
        /// </summary>
        public static SpriteFont ArialProp 
        {
            get { return arial; }
        }

        /// <summary>
        /// Moves with the player. Draws everything else in relation to this
        /// </summary>
        public static Vector2 CameraPositionProp { get => cameraPosition; set => cameraPosition = value; }

        /// <summary>
        /// Size of screen for positioning player in the middle
        /// </summary>
        public static Vector2 ScreenSizeProp { get => screenSize; set => screenSize = value; }

        /// <summary>
        /// Easily accessable reference to the instatiated player
        /// </summary>
        public static Player PlayerProp { get => player; }

        /// <summary>
        /// Easily accessable reference to the instatiated boss
        /// </summary>
        public static Boss BossProp { get => boss; }

        /// <summary>
        /// Manages game states
        /// </summary>
        public static GameState WinLoseStateProp
        {
            get { return winLoseState; }
            set { winLoseState = value; }
        }

        /// <summary>
        /// Screensize is fixed to 1600x900
        /// </summary>
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Adds the player and boss to the gameObjects List, also loads the stage.
        /// </summary>
        protected override void Initialize()
        {
            winLoseState = GameState.Play;
            gameObjects.Add(PlayerProp);
            gameObjects.Add(BossProp);
            gameObjects.Add(new Target());

            base.Initialize();
            Stage.LoadLevel("PrototypePlayground");
        }

        /// <summary>
        /// Runs the LoadContent in every object inside the gameObject List, also plays the backgroundmusic on loop.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>("Arial");

            backgroundMusic = Content.Load<SoundEffect>("music").CreateInstance();
            backgroundMusic.Volume = 0.1f;
            backgroundMusic.Play();
            backgroundMusic.IsLooped = true;

            collisionTexture = Content.Load<Texture2D>("collisionTexture");

            // Sprites used in the stage have to be loaded here
            Stage.LoadContent(Content);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }

            UI.LoadContent(Content);
        }

        /// <summary>
        /// Runs the Update() method on every gameObject & adds / removes objects from the game.
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
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
                UI.Update();
            }            

            base.Update(gameTime);
        }

        /// <summary>
        /// Runs all relevant draw methods on gameobjects
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
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
                spriteBatch.DrawString(ArialProp, "You Win", new Vector2(screenSize.X / 2 - 100, screenSize.Y / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }
            else if (winLoseState == GameState.Lose)
            {
                spriteBatch.DrawString(ArialProp, "You Suck", new Vector2(screenSize.X / 2 - 100, screenSize.Y / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds newly instantiated gameobjects to gameobject list
        /// </summary>
        /// <param name="gameObject">Gameobject to be added</param>
        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes gameobject from gameobject list
        /// </summary>
        /// <param name="gameObject">gameobject to be removed</param>
        public static void Destroy(GameObject gameObject)
        {
            removeGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Runs the drawBox code with the given Rectangle from its parameter
        /// </summary>
        /// <param name="rect">Box to draw</param>
        private void DrawCollisionBox(Rectangle rect)
        {
            DrawBox(rect, Color.Red, 1);
        }

        /// <summary>
        /// Draws the World Boundary in DarkGray colour.
        /// </summary>
        /// <param name="rect">Worldsize</param>
        private void DrawWorldBoundary(Rectangle rect)
        {
            Rectangle collisionBox = rect;

            collisionBox.X = collisionBox.X - (int)CameraPositionProp.X + (int)ScreenSizeProp.X / 2;
            collisionBox.Y = collisionBox.Y - (int)CameraPositionProp.Y + (int)ScreenSizeProp.Y / 2;

            DrawBox(collisionBox, Color.DarkGray, 10);
        }

        /// <summary>
        /// Draws a box around every rectangle. The rectangle is in most cases the same size as the sprite.
        /// </summary>
        /// <param name="rect">Box to draw</param>
        /// <param name="color">Color of lines</param>
        /// <param name="lineWidth">Width of lines</param>
        private void DrawBox(Rectangle rect, Color color, int lineWidth)
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
