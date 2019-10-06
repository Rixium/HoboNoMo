using Microsoft.Xna.Framework.Content;

namespace HoboNoMo
{
    public class ContentChest
    {
        private ContentManager _contentManager;

        private ContentChest(ContentManager manager)
        {
            _contentManager = manager;
        }

        public static ContentChest Create(ContentManager manager)
        {
            return new ContentChest(manager);
        }
    }
}