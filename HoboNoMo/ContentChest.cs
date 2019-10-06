using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HoboNoMo
{
    public class ContentChest
    {
        private ContentManager _contentManager;

        public Texture2D Splash { get; private set; }
        public SpriteFont TitleFont { get; private set; }
        public Texture2D Pixel { get; set; }
        public SpriteFont ButtonFont { get; set; }
        public Texture2D SelectionIcon { get; set; }
        public SoundEffect Select { get; set; }
        public SoundEffect Ho { get; set; }
        public SoundEffect Bo { get; set; }
        
        public Song MainTheme { get; set; }

        private ContentChest(ContentManager manager)
        {
            _contentManager = manager;
        }

        public static ContentChest Create(ContentManager manager)
        {
            return new ContentChest(manager);
        }

        public void Load()
        {
            Pixel = _contentManager.Load<Texture2D>("Core/Pixel");
            Splash = _contentManager.Load<Texture2D>("Core/Logo");

            TitleFont = _contentManager.Load<SpriteFont>("Fonts/TitleFont");
            ButtonFont = _contentManager.Load<SpriteFont>("Fonts/ButtonFont");

            SelectionIcon = _contentManager.Load<Texture2D>("UI/Arrow");

            Select = _contentManager.Load<SoundEffect>("Sounds/select");
            Ho = _contentManager.Load<SoundEffect>("Sounds/ho");
            Bo = _contentManager.Load<SoundEffect>("Sounds/bo");

            MainTheme = _contentManager.Load<Song>("Music/hobotheme");
        }
    }
}