using System;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Scenes
{
    public interface IScene
    {
        Func<IScene, bool> RequestSceneChange { get; set; }
        void Update(float delta);
        void Draw(SpriteBatch spriteBatch);
    }
}