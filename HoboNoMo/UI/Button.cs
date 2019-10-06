using System;
using HoboNoMo.Helpers;
using HoboNoMo.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.UI
{
    public class Button
    {
        private Texture2D _selectionIcon;

        public bool Selected
        {
            get => _selected;
            set
            {
                if (value == _selected) return;
                _selected = value;

                if (_selected)
                {
                    OnButtonSelected?.Invoke(this);
                }
            }
        }

        public Action<Button> OnButtonSelected { get; set; }

        private readonly SpriteFont _font;
        private readonly string _text;
        private readonly Vector2 _position;
        private readonly Color _backColor;
        private readonly Color _textColor;
        private readonly Alignment _alignment;
        private Rectangle _rectangle;
        private Vector2 _textPosition;
        private bool _selected;
        private int _startScreenWidth;
        private int _startScreenHeight;

        public Button(SpriteFont font, string text, Vector2 position, Color backColor, Color textColor,
            Alignment alignment, Texture2D selectionIcon, bool selected = false)
        {
            _font = font;
            _text = text;
            _position = position;
            _backColor = backColor;
            _textColor = textColor;
            _alignment = alignment;
            _selectionIcon = selectionIcon;
            _selected = selected;

            _startScreenWidth = UserPreferences.ScreenWidth;
            _startScreenHeight = UserPreferences.ScreenHeight;
            
            Game1.OnScreenSizeChanged += Initialise;
            
            Initialise();
        }

        private void Initialise()
        {
            var measurements = StringHelpers.Measure(_text, _font);
            if (_alignment == Alignment.Left)
            {
                _rectangle = new Rectangle((int) _position.X - _startScreenWidth + UserPreferences.ScreenWidth, (int) _position.Y - (_startScreenHeight - UserPreferences.ScreenHeight), (int) measurements.X + 20,
                    (int) measurements.Y + 20);
            }
            else if (_alignment == Alignment.Right)
            {
                _rectangle = new Rectangle((int) _position.X - (int) measurements.X - 20 - _startScreenWidth + UserPreferences.ScreenWidth, (int) _position.Y - (_startScreenHeight - UserPreferences.ScreenHeight),
                    (int) measurements.X + 20,
                    (int) measurements.Y + 20);
            }
            else if (_alignment == Alignment.Center)
            {
                _rectangle = new Rectangle((int) (_position.X - measurements.X / 2 - 10 - _startScreenWidth + UserPreferences.ScreenWidth),
                    (int) (_position.Y - measurements.Y / 2 - 10 - _startScreenHeight + UserPreferences.ScreenHeight), (int) measurements.X + 20,
                    (int) measurements.Y + 20);
            }

            _textPosition = new Vector2(_rectangle.X + 10, _rectangle.Y + 10);
            
            _startScreenWidth = UserPreferences.ScreenWidth;
            _startScreenHeight = UserPreferences.ScreenHeight;
        }

        public void Update(float delta)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteHelper.DrawRectangle(spriteBatch, _rectangle, _backColor);
            spriteBatch.DrawString(_font, _text, _textPosition, _textColor);

            if (Selected)
            {
                spriteBatch.Draw(_selectionIcon, 
                    new Vector2(_rectangle.X - 10 - _selectionIcon.Width, _rectangle.Y + _rectangle.Height / 2 - _selectionIcon.Height / 2), 
                    Color.White);
            }
        }
    }
}