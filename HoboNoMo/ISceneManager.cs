using HoboNoMo.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo
{
    public interface ISceneManager
    {
        IScene ActiveScene { get; }
        void Update(float delta);
        void Draw(SpriteBatch spriteBatch);
    }
}