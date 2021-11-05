using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceySpaceyShootShoot
{
    public class GameWorld : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont text;

        private List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> starList = new List<GameObject>(); //Used for background objects
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private Texture2D collisionTexture;
        private Song backgroundMusic;

        private static int points = 0;

        private static Vector2 screenSize;

        public static Vector2 ScreenSize { get => screenSize; set => screenSize = value; }
        public static int Points { get => points; set => points = value; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 900;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        Enemy enemy = new Enemy(); //Used in spawner function

        protected override void Initialize()
        {
            gameObjects = new List<GameObject>();
            newGameObjects = new List<GameObject>();
            removeGameObjects = new List<GameObject>();
            Player player = new Player();
            gameObjects.Add(player);
            gameObjects.Add(new Enemy());
            gameObjects.Add(new Enemy());

            for (int i = 0; i < 1000; i++) //add stars for scrolling background
            {
                starList.Add(new Star());
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundMusic = Content.Load<Song>("music");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            text = Content.Load<SpriteFont>("text");

            collisionTexture = Content.Load<Texture2D>("SpaceShooterSprites/CollisionTexture");

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }
            foreach (GameObject star in starList)
            {
                star.LoadContent(Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            AddObjects();
            RemoveObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
                foreach (GameObject other in gameObjects)
                {
                    gameObject.CheckCollision(other);
                }
            }

            foreach (GameObject star in starList)
            {
                star.Update(gameTime);
            }

            //SpawnEnemy(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Adds objects to the gameObjects list, from newGameObjects List.
        /// </summary>
        public void AddObjects()
        {
            gameObjects.AddRange(newGameObjects);
            newGameObjects.Clear(); //Clear the list after objects have been added
        }

        /// <summary>
        /// Removes objects from gameObjects list, from removeGameObjects List.
        /// </summary>
        public void RemoveObjects()
        {
            foreach (GameObject gameObject in removeGameObjects)
            {
                gameObjects.Remove(gameObject);
            }
            removeGameObjects.Clear();
        }

        /// <summary>
        /// Adds gameObjects to newGameObjects List. Use AddObjects to add to gameObjects List.
        /// </summary>
        /// <param name="gameObject"></param>
        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Adds gameObjects to removeGameObjects List. Use RemoveObjects to remove from gameObjects List.
        /// </summary>
        /// <param name="gameObject"></param>
        public static void Destroy(GameObject gameObject)
        {
            removeGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Spawns new enemies, by adding new gameObjects of Enemy type.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool SpawnTimer(GameTime gameTime) //Old spawn method
        {
            float currentTime = 0;
            float spawnTime = 5;
            if (currentTime <= 0)
            {
                currentTime = spawnTime;
                return true;
            }
            else
            {
                currentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return false;
            }
        }

        /// <summary>
        /// Spawns new enemies by creating new gameObjects.
        /// </summary>
        /// <param name="gameTime"></param>
        public void SpawnEnemy(GameTime gameTime) //Old spawn method
        {
            if(SpawnTimer(gameTime) == true)
            {
                newGameObjects.Add(enemy);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            //UI
            _spriteBatch.DrawString(text, Points.ToString(), new Vector2(ScreenSize.X/2, 50), Color.White);

            //GameObjects
            foreach (GameObject star in starList)
            {
                star.Draw(_spriteBatch);
            }

            foreach (GameObject obj in gameObjects)
            {
                obj.Draw(_spriteBatch);
#if DEBUG
                DrawCollisionBox(obj);
#endif
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// Draws collisionboxes around every gameObject, only used in Debug mode.
        /// </summary>
        /// <param name="gameObject"></param>
        private void DrawCollisionBox(GameObject gameObject)
        {
            Rectangle collisionBox = gameObject.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            _spriteBatch.Draw(collisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}
