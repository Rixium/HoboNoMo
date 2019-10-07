using System;
using System.Diagnostics;
using HoboNoMo.Input;
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
        public static Action OnQuit;

        public Game1()
        {
            Debug.WriteLine("Hello World!");
            LoadUserPreferences();

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = UserPreferences.ScreenWidth,
                PreferredBackBufferHeight = UserPreferences.ScreenHeight
            };

            Content.RootDirectory = "Content";

            ContentChest = ContentChest.Create(Content);
            _sceneManager = new SceneManager();

            OnQuit += Exit;
        }

        public static string GameTitle { get; set; } = "Hobonomo";
        public static string Version { get; set; } = "1.0.0";

        private static void LoadUserPreferences()
        {
            UserPreferences.Load();

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                ConfigureController();
            }
            else ConfigureKeyboard();
        }

        private static void ConfigureKeyboard()
        {
            InputManager.AddBinding(InputManager.Binding.Player1Up,
                new KeyInputBinding(Keys.W));

            InputManager.AddBinding(InputManager.Binding.Player1Down,
                new KeyInputBinding(Keys.S));
            
            
            InputManager.AddBinding(InputManager.Binding.Player1Left,
                new KeyInputBinding(Keys.A));

            InputManager.AddBinding(InputManager.Binding.Player1Right,
                new KeyInputBinding(Keys.D));

            
            InputManager.AddBinding(InputManager.Binding.Player1Use,
                new KeyInputBinding(Keys.Space));
        }

        private static void ConfigureController()
        {
            InputManager.AddBinding(InputManager.Binding.Player1Up,
                new GamePadThumbstickInputBinding(PlayerIndex.One,
                    GamePadThumbstickInputBinding.Thumbstick.Left,
                    GamePadThumbstickInputBinding.Direction.Up));

            InputManager.AddBinding(InputManager.Binding.Player1Down,
                new GamePadThumbstickInputBinding(PlayerIndex.One,
                    GamePadThumbstickInputBinding.Thumbstick.Left,
                    GamePadThumbstickInputBinding.Direction.Down));
            
            
            InputManager.AddBinding(InputManager.Binding.Player1Use,
                new GamePadButtonInputBinding(PlayerIndex.One, Buttons.A));
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

            InputManager.Update(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
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