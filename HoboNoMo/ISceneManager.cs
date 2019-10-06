using System;
using HoboNoMo.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo
{
    public interface ISceneManager
    {
        Action OnSceneChange { get; set; }
        IScene ActiveScene { get; }
        void Update(float delta);
        void Draw(SpriteBatch spriteBatch);
    }
}