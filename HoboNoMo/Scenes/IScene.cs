using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Scenes
{
    public interface IScene
    {
        void Update(float delta);
        void Draw(SpriteBatch spriteBatch);
    }
}