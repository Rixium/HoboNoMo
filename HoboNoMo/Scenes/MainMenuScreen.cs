using System;
using System.Collections.Generic;
using HoboNoMo.Helpers;
using HoboNoMo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HoboNoMo.Scenes
{
    public class MainMenuScreen : IScene
    {
        private readonly ContentChest _contentChest;
        public Func<IScene, bool> RequestSceneChange { get; set; }

        private Vector2 _titlePosition;
        private Color _titleColor = Color.White;
        private string _titleText = "Hobo No Mo'";

        private Color _noColor = Color.White * 0.0f;
        private Button _playButton;
        private Button _quitButton;
        private Button _optionsButton;
        private Color _footerColor = Color.White * 0.8f;
        private Vector2 _footerPosition;
        private string _footerText = "A game made for Ludum Dare 45";
        private List<Button> _buttons;

        private int _selectedButton;
        private KeyboardState _lastKeyState;

        private int _currentSound;

        public MainMenuScreen(ContentChest contentChest)
        {
            Game1.OnScreenSizeChanged += Initialise;
            
            _contentChest = contentChest;
            MediaPlayer.Play(_contentChest.MainTheme);
            MediaPlayer.IsRepeating = true;
            Initialise();
        }

        private void OnButtonSelect(Button button)
        {
            _buttons.ForEach(b =>
            {
                if (b != button)
                {
                    b.Selected = false;
                }
            });

            if (_currentSound == 0)
                _contentChest.Ho.Play();
            else
                _contentChest.Bo.Play();

            _currentSound = _currentSound == 1 ? 0 : 1;
        }

        private void Initialise()
        {
            _titlePosition = StringHelpers.Center(_contentChest.TitleFont, _titleText, UserPreferences.ScreenWidth,
                UserPreferences.ScreenHeight );
            _titlePosition.Y = UserPreferences.ScreenHeight / 3.0f;
            
            var (x, y) = StringHelpers.Measure(_footerText, _contentChest.ButtonFont);
            _footerPosition = new Vector2(UserPreferences.ScreenWidth / 2.0f - x / 2,
                UserPreferences.ScreenHeight - 10 - y);
            
            
            var playButton = new Button(_contentChest.ButtonFont,
                "Start",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon, true);

            playButton.OnButtonSelected += OnButtonSelect;

            var optionsButton = new Button(_contentChest.ButtonFont,
                "Options",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 30),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon);

            optionsButton.OnButtonSelected += OnButtonSelect;

            var quitButton = new Button(_contentChest.ButtonFont,
                "Quit",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 60),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon);

            quitButton.OnButtonSelected += OnButtonSelect;

            _buttons = new List<Button>(new[] {playButton, optionsButton, quitButton});
        }

        public void Update(float delta)
        {
            var keyState = Keyboard.GetState();

            _buttons.ForEach(b => b.Update(delta));
            var changed = false;
            if (keyState.IsKeyDown(Keys.S) && _lastKeyState.IsKeyUp(Keys.S))
            {
                _selectedButton++;
                changed = true;
            }
            else if (keyState.IsKeyDown(Keys.W) && _lastKeyState.IsKeyUp(Keys.W))
            {
                _selectedButton--;
                changed = true;
            }

            if (changed)
            {
                if (_selectedButton < 0)
                    _selectedButton = _buttons.Count - 1;
                if (_selectedButton >= _buttons.Count)
                    _selectedButton = 0;
                
                _buttons[_selectedButton].Selected = true;
            }

            _lastKeyState = keyState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp);

            spriteBatch.DrawString(_contentChest.TitleFont, _titleText, _titlePosition, _titleColor);

            _buttons.ForEach(b => b.Draw(spriteBatch));

            spriteBatch.DrawString(_contentChest.ButtonFont, _footerText, _footerPosition, _footerColor);
            spriteBatch.End();
        }
    }

    public enum Alignment
    {
        Center,
        Left,
        Right
    }
}