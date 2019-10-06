using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Scenes
{
    public class SplashScreen : IScene
    {
        private readonly ContentChest _contentChest;

        private Rectangle _splashRectangle;
        private bool _requested;

        public SplashScreen(ContentChest contentChest)
        {
            Game1.OnScreenSizeChanged += Initialise;
            _contentChest = contentChest;
            
            Initialise();
        }

        private void Initialise()
        {
            _splashRectangle = new Rectangle(0, 0, UserPreferences.ScreenWidth, UserPreferences.ScreenHeight);
        }

        public Func<IScene, bool> RequestSceneChange { get; set; }

#if DEBUG
        private float _showTime = 1.0f;
#else
        private float _showTime = 3.0f;
#endif

        public void Update(float delta)
        {
            _showTime -= delta;

            if (_showTime > 0) return;
            if (_requested) return;
            _requested = RequestSceneChange(new MainMenuScreen(_contentChest));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.Draw(_contentChest.Splash,
                _splashRectangle, Color.White);
            spriteBatch.End();
        }
    }
}