using System;
using System.Collections.Generic;
using HoboNoMo.Helpers;
using HoboNoMo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboNoMo.Scenes
{
    public class ConnectionScreen : IScene
    {
        private readonly ContentChest _contentChest;

        private string _joinTitle = "Join";
        private Vector2 _joinPosition;
        private string _footerText = "A game made for Ludum Dare 45";
        private Vector2 _footerPosition;

        private List<Button> _buttons = new List<Button>();
        private Color _noColor = Color.White * 0.0f;
        private int _selectedButton;
        private int _currentSound;

        private string _ipAddress = "";
        private KeyboardState _lastState;
        private bool _cursorActive;
        private float _cursorTimer;

        public Func<IScene, bool> RequestSceneChange { get; set; }

        public ConnectionScreen(ContentChest contentChest)
        {
            _contentChest = contentChest;
            Game1.OnScreenSizeChanged += Initialise;
            Initialise();
        }

        private void Initialise()
        {
            _joinPosition = StringHelpers.Center(_contentChest.TitleFont, _joinTitle, UserPreferences.ScreenWidth,
                UserPreferences.ScreenHeight);
            _joinPosition.Y = UserPreferences.ScreenHeight / 3.0f;

            var (x, y) = StringHelpers.Measure(_footerText, _contentChest.ButtonFont);
            _footerPosition = new Vector2(UserPreferences.ScreenWidth / 2.0f - x / 2,
                UserPreferences.ScreenHeight - 10 - y);

            var connectButton = new Button(_contentChest.ButtonFont,
                "Connect",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 60),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon, true);

            connectButton.OnButtonSelected += OnButtonSelect;

            connectButton.OnClick += () =>
            {
                RequestSceneChange?.Invoke(new LobbyScreen(_contentChest, true, _ipAddress));
            };

            var backButton = new Button(_contentChest.ButtonFont,
                "Back",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 90),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon);

            backButton.OnButtonSelected += OnButtonSelect;

            backButton.OnClick += () => { RequestSceneChange?.Invoke(new MainMenuScreen(_contentChest)); };

            _buttons = new List<Button>(new[] {connectButton, backButton});
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

        public void Update(float delta)
        {
            _cursorTimer += delta;

            if (_cursorTimer >= 0.5f)
            {
                _cursorActive = !_cursorActive;
                _cursorTimer = 0;
            }
            
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.D0) && _lastState.IsKeyUp(Keys.D0))
                _ipAddress += "0";
            else if (keyState.IsKeyDown(Keys.D1) && _lastState.IsKeyUp(Keys.D1))
                _ipAddress += "1";
            else if (keyState.IsKeyDown(Keys.D2) && _lastState.IsKeyUp(Keys.D2))
                _ipAddress += "2";
            else if (keyState.IsKeyDown(Keys.D3) && _lastState.IsKeyUp(Keys.D3))
                _ipAddress += "3";
            else if (keyState.IsKeyDown(Keys.D4) && _lastState.IsKeyUp(Keys.D4))
                _ipAddress += "4";
            else if (keyState.IsKeyDown(Keys.D5) && _lastState.IsKeyUp(Keys.D5))
                _ipAddress += "5";
            else if (keyState.IsKeyDown(Keys.D6) && _lastState.IsKeyUp(Keys.D6))
                _ipAddress += "6";
            else if (keyState.IsKeyDown(Keys.D7) && _lastState.IsKeyUp(Keys.D7))
                _ipAddress += "7";
            else if (keyState.IsKeyDown(Keys.D8) && _lastState.IsKeyUp(Keys.D8))
                _ipAddress += "8";
            else if (keyState.IsKeyDown(Keys.D9) && _lastState.IsKeyUp(Keys.D9))
                _ipAddress += "9";
            else if (keyState.IsKeyDown(Keys.OemPeriod) && _lastState.IsKeyUp(Keys.OemPeriod))
                _ipAddress += ".";
            else if (keyState.IsKeyDown(Keys.Back) && _lastState.IsKeyUp(Keys.Back))
                if (_ipAddress.Length > 0)
                {
                    _ipAddress = _ipAddress.Substring(0, _ipAddress.Length - 1);
                }
            
            _lastState = keyState;
            
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
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.DrawString(_contentChest.TitleFont, _joinTitle, _joinPosition, Color.White);
            spriteBatch.DrawString(_contentChest.ButtonFont, _footerText, _footerPosition, Color.White);

            var ipSize = StringHelpers.Measure(_ipAddress, _contentChest.ButtonFont);
            var ipPos = new Vector2(UserPreferences.ScreenWidth / 2.0f - ipSize.X / 2,
                UserPreferences.ScreenHeight / 2.0f - ipSize.Y / 2);
            spriteBatch.DrawString(_contentChest.ButtonFont, _ipAddress, ipPos, Color.White);

            if (_cursorActive)
            {
                spriteBatch.DrawString(_contentChest.ButtonFont, "|", new Vector2(ipPos.X + ipSize.X + 1, ipPos.Y), Color.White);
            }
            _buttons.ForEach(b => b.Draw(spriteBatch));

            spriteBatch.End();
        }
    }
}