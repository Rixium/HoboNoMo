using System;
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
            set
            {
                value.RequestSceneChange = OnSceneChangeRequest;
                _activeScene = value;
            }
        }

        public Action OnSceneChange { get; set; }

        private bool OnSceneChangeRequest(IScene scene)
        {
            ActiveScene = scene;
            return true;
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