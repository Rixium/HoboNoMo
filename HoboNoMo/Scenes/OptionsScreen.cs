using System;
using System.Collections.Generic;
using HoboNoMo.Helpers;
using HoboNoMo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Scenes
{
    public class OptionsScreen : IScene
    {
        private readonly ContentChest _contentChest;

        private string _optionsTitle = "Options";
        private Vector2 _optionsPosition;
        private string _footerText = "A game made for Ludum Dare 45";
        private Vector2 _footerPosition;

        private List<Button> _buttons = new List<Button>();
        private Color _noColor = Color.White * 0.0f;
        private int _selectedButton;
        private int _currentSound;

        public OptionsScreen(ContentChest contentChest)
        {
            _contentChest = contentChest;
            Game1.OnScreenSizeChanged += Initialise;
            Initialise();
        }

        private void Initialise()
        {
            _optionsPosition = StringHelpers.Center(_contentChest.TitleFont, _optionsTitle, UserPreferences.ScreenWidth,
                UserPreferences.ScreenHeight);
            _optionsPosition.Y = UserPreferences.ScreenHeight / 3.0f;

            var (x, y) = StringHelpers.Measure(_footerText, _contentChest.ButtonFont);
            _footerPosition = new Vector2(UserPreferences.ScreenWidth / 2.0f - x / 2,
                UserPreferences.ScreenHeight - 10 - y);
            
            
            var backButton = new Button(_contentChest.ButtonFont,
                "Back",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 90),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon, true);

            backButton.OnButtonSelected += OnButtonSelect;

            backButton.OnClick += () => { RequestSceneChange?.Invoke(new MainMenuScreen(_contentChest)); };

            _buttons = new List<Button>(new [] { backButton });
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
        
        public Func<IScene, bool> RequestSceneChange { get; set; }

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
            } else if (InputManager.Player1Use.Press)
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
            spriteBatch.DrawString(_contentChest.TitleFont, _optionsTitle, _optionsPosition, Color.White);
            spriteBatch.DrawString(_contentChest.ButtonFont, _footerText, _footerPosition, Color.White);
            
            _buttons.ForEach(b => b.Draw(spriteBatch));
            
            spriteBatch.End();
        }
    }
}