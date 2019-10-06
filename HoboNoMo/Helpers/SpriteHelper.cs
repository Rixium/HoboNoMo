using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Helpers
{
    public class SpriteHelper
    {
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(Game1.ContentChest.Pixel, rectangle, color);
        }
    }
}