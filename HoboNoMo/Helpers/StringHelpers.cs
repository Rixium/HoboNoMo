using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Helpers
{
    public class StringHelpers
    {
        public static Vector2 Center(SpriteFont font, string text, int width, int height)
        {
            var (x, y) = font.MeasureString(text);
            return new Vector2(width / 2.0f - x / 2, height / 2.0f - y / 2);
        }

        public static Vector2 Measure(string text, SpriteFont font)
        {
            return font.MeasureString(text);
        }
    }
}