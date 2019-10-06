using HoboNoMo.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo
{
    public class SceneManager : ISceneManager
    {
        private IScene _activeScene;

        public IScene ActiveScene
        {
            get => _activeScene;
            set => _activeScene = value;
        }

        public void Update(float delta)
        {
            ActiveScene?.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ActiveScene?.Draw(spriteBatch);
        }
    }
}