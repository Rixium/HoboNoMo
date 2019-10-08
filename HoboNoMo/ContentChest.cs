using System.Collections.Generic;
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

        public enum Outfit
        {
            Nude,
            One,
            Two,
            Three,
            Four,
            Five,
            Six
        }
        
        public Dictionary<Outfit, Dictionary<Player.Animation, Texture2D[]>> PlayerTextures { get; set; } = new Dictionary<Outfit, Dictionary<Player.Animation, Texture2D[]>>();

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

            for (var i = Outfit.Nude; i <= Outfit.Six; i++)
            {
                
                var idle = new[]
                {
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1frontStand")
                };
                var walkLeft = new[]
                {
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1leftWalk1"),
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1leftWalk2")
                };
                var walkRight = new[]
                {
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1rightWalk1"),
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1rightWalk2")
                };
                var walkDown = new[]
                {
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1frontWalk1"),
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1frontWalk2")
                };

                var walkUp = new[]
                {
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1backWalk1"),
                    _contentManager.Load<Texture2D>($"Player/Outfit{(int)(i + 1)}/1backWalk2")
                };
                
                var newDictionary = new Dictionary<Player.Animation, Texture2D[]>
                {
                    {Player.Animation.Idle, idle},
                    {Player.Animation.WalkLeft, walkLeft},
                    {Player.Animation.WalkRight, walkRight},
                    {Player.Animation.WalkDown, walkDown},
                    {Player.Animation.WalkUp, walkUp}
                };
                
                PlayerTextures.Add(i, newDictionary);
            }
            
        }
    }
}