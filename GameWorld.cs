using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PixelSouls
{
    public enum GameState { Play, Menu, Lose, Win}

    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D collisionTexture;
        public SpriteFont arial;

        public static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static Vector2 screenSize;
        private static Vector2 cameraPosition = new Vector2(500, 500);

        public static Player player = new Player();

        private static GameState winLoseState;

        private ButtonState leftMouseButton;

        private List<UI> uiElements;

        private UI ui;

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
            //Window.AllowUserResizing = true;
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Window.ClientSizeChanged += OnResize;
        }

        private void OnResize (object sender, EventArgs args)
        {
            //When the window size is changed we have to update the screenSize field
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        protected override void Initialize()
        {
            gameObjects.Add(player);
            ui = new UI();
            base.Initialize();
            Stage.LoadLevel("PrototypePlayground");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            collisionTexture = Content.Load<Texture2D>("solid");

            //Sprites used in the stage have to be loaded here
            Stage.loadContent(Content);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }
            ui.LoadContent(Content);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameObjects.AddRange(newGameObjects);
            newGameObjects.Clear();

            foreach (GameObject gameObject in removeGameObjects)
            {
                gameObjects.Remove(gameObject);
                //removeGameObjects.Clear();
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
            ui.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack); //Makes layers work

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
                #if DEBUG
                DrawCollisionBox(gameObject);
                #endif
            }
            ui.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool LeftMouseButtonReleased()
        {
            // placeholder code
            return true;
        }

        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        public static void Destroy(GameObject gameObject)
        {
            removeGameObjects.Add(gameObject);
        }

        private void DrawCollisionBox(GameObject localGameObject)
        {
            Rectangle collisionBox = localGameObject.CollisionBoxProp;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            spriteBatch.Draw(collisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}
