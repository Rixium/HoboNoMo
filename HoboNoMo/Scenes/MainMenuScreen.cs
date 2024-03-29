using System;
using System.Collections.Generic;
using HoboNoMo.Helpers;
using HoboNoMo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Color _footerColor = Color.White * 0.8f;
        private Vector2 _footerPosition;
        private string _footerText = "A game made for Ludum Dare 45";
        private List<Button> _buttons;

        private int _selectedButton;

        private int _currentSound;

        public MainMenuScreen(ContentChest contentChest)
        {
            Game1.OnScreenSizeChanged += Initialise;

            _contentChest = contentChest;

            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.Play(_contentChest.MainTheme);
                MediaPlayer.IsRepeating = true;
            }

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
                UserPreferences.ScreenHeight);
            _titlePosition.Y = UserPreferences.ScreenHeight / 3.0f;

            var (x, y) = StringHelpers.Measure(_footerText, _contentChest.ButtonFont);
            _footerPosition = new Vector2(UserPreferences.ScreenWidth / 2.0f - x / 2,
                UserPreferences.ScreenHeight - 10 - y);


            var playButton = new Button(_contentChest.ButtonFont,
                "Create",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon, true);

            playButton.OnButtonSelected += OnButtonSelect;

            playButton.OnClick += () => { RequestSceneChange?.Invoke(new LobbyScreen(_contentChest)); };
            
            var joinButton = new Button(_contentChest.ButtonFont,
                "Join",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 30),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon);

            joinButton.OnButtonSelected += OnButtonSelect;

            joinButton.OnClick += () => { RequestSceneChange?.Invoke(new ConnectionScreen(_contentChest)); };

            
            var optionsButton = new Button(_contentChest.ButtonFont,
                "Options",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 60),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon);

            optionsButton.OnButtonSelected += OnButtonSelect;

            optionsButton.OnClick += () => { RequestSceneChange?.Invoke(new OptionsScreen(_contentChest)); };
            
            var quitButton = new Button(_contentChest.ButtonFont,
                "Quit",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 90),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon);

            quitButton.OnButtonSelected += OnButtonSelect;

            quitButton.OnClick += () => { Game1.OnQuit?.Invoke(); };

            _buttons = new List<Button>(new[] {playButton, joinButton, optionsButton, quitButton});
        }

        public void Update(float delta)
        {
            _buttons.ForEach(b => b.Update(delta));
            var changed = false;
            if (InputManager.Player1Down.Press)
            {
                _selectedButton++;
                changed = true;
            }
            else if (InputManager.Player1Up.Press)
            {
                _selectedButton--;
                changed = true;
            }
            else if (InputManager.Player1Use.Press)
            {
                _buttons[_selectedButton].OnClick?.Invoke();
            }

            if (!changed) return;

            if (_selectedButton < 0)
                _selectedButton = _buttons.Count - 1;
            if (_selectedButton >= _buttons.Count)
                _selectedButton = 0;

            _buttons[_selectedButton].Selected = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp);

            spriteBatch.DrawString(_contentChest.TitleFont, _titleText, _titlePosition, _titleColor);

            _buttons.ForEach(b => b.Draw(spriteBatch));

            spriteBatch.DrawString(_contentChest.ButtonFont, _footerText, _footerPosition, Color.White);
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