using System;
using HoboNoMo.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboNoMo
{
    public class Game1 : Game
    {
        private string _gameTitle = "Hobo No Mo'";
        
        static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static ContentChest ContentChest;
        private SceneManager _sceneManager;
        
        public static Action OnScreenSizeChanged;

        public Game1()
        {
            LoadUserPreferences();

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = UserPreferences.ScreenWidth,
                PreferredBackBufferHeight = UserPreferences.ScreenHeight
            };

            Content.RootDirectory = "Content";

            ContentChest = ContentChest.Create(Content);
            _sceneManager = new SceneManager();
        }

        private static void LoadUserPreferences()
        {
            UserPreferences.Load();
        }

        protected override void Initialize()
        {
            Window.Title = _gameTitle;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ContentChest.Load();

            _sceneManager.OnSceneChange += OnSceneChange;
            _sceneManager.ActiveScene = new SplashScreen(ContentChest);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _sceneManager.Update(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _sceneManager.Draw(spriteBatch);
            
            base.Draw(gameTime);
        }

        public void OnSceneChange()
        {
            OnScreenSizeChanged = null;
        }
        
        public static void SetScreenSize(int width, int height)
        {
            UserPreferences.ScreenHeight = height;
            UserPreferences.ScreenWidth = width;
            graphics.PreferredBackBufferHeight = UserPreferences.ScreenHeight;
            graphics.PreferredBackBufferWidth = UserPreferences.ScreenWidth;
            graphics.ApplyChanges();
            OnScreenSizeChanged?.Invoke();
        }
    }

}