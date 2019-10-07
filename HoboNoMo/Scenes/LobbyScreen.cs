using System;
using System.Collections.Generic;
using HoboNoMo.Helpers;
using HoboNoMo.Network;
using HoboNoMo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Scenes
{
    public class LobbyScreen : IScene
    {
        private readonly ContentChest _contentChest;

        private string _lobbyTitle = "Lobby";
        private Vector2 _lobbyPosition;
        private string _footerText = "A game made for Ludum Dare 45";
        private Vector2 _footerPosition;

        private List<Button> _buttons = new List<Button>();
        private Color _noColor = Color.White * 0.0f;
        private int _selectedButton;
        private int _currentSound;

        private NetworkManager _networkManager;

        private List<Color> _colors = new List<Color>(new[]
        {
            Color.Blue,
            Color.Red,
            Color.Green,
            Color.Yellow,
            Color.Purple
        });
        
        public LobbyScreen(ContentChest contentChest, bool joining = false, string ipAddress = "")
        {
            _contentChest = contentChest;
            Game1.OnScreenSizeChanged += Initialise;
            Initialise();

            _networkManager = new NetworkManager();

            if (joining == false)
            {
                _networkManager.OnPlayerAdded += player => { _contentChest.Select.Play(); };
                _networkManager.Create();
                _networkManager.AddPlayer(new Player
                {
                    Id = Guid.NewGuid(),
                    Name = "Player" + _networkManager.Players.Count + 1
                }, true);
            }
            else
            {
                _networkManager.Join(ipAddress, 27411);
                _networkManager.OnPlayerAdded += player => { _contentChest.Select.Play(); };
                _networkManager.OnConnected += () => { _contentChest.Select.Play(); };
            }
        }

        private void Initialise()
        {
            _lobbyPosition = StringHelpers.Center(_contentChest.TitleFont, _lobbyTitle, UserPreferences.ScreenWidth,
                UserPreferences.ScreenHeight);
            _lobbyPosition.Y = UserPreferences.ScreenHeight / 3.0f;

            var (x, y) = StringHelpers.Measure(_footerText, _contentChest.ButtonFont);
            _footerPosition = new Vector2(UserPreferences.ScreenWidth / 2.0f - x / 2,
                UserPreferences.ScreenHeight - 10 - y);
            
            
            var backButton = new Button(_contentChest.ButtonFont,
                "Back",
                new Vector2(UserPreferences.ScreenWidth / 2.0f, UserPreferences.ScreenHeight / 2.0f + 90),
                _noColor, Color.White, Alignment.Center, _contentChest.SelectionIcon, true);

            backButton.OnButtonSelected += OnButtonSelect;

            backButton.OnClick += () =>
            {
                _networkManager.Disconnect();
                RequestSceneChange?.Invoke(new MainMenuScreen(_contentChest));
            };

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
            _networkManager.Update(delta);
            
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
            spriteBatch.DrawString(_contentChest.TitleFont, _lobbyTitle, _lobbyPosition, Color.White);
            spriteBatch.DrawString(_contentChest.ButtonFont, _footerText, _footerPosition, Color.White);
            
            spriteBatch.DrawString(_contentChest.ButtonFont, $"IP: {_networkManager.IpAddress}", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(_contentChest.ButtonFont, $"Port: {_networkManager.Port}", new Vector2(10, 50), Color.White);

            for (var i = 0; i < _networkManager.Players.Count; i++)
            {
                var player = _networkManager.Players[i];
                var measurements = StringHelpers.Measure(player.Name, _contentChest.ButtonFont);
                var position = new Vector2(UserPreferences.ScreenWidth / 2.0f - measurements.X / 2, _lobbyPosition.Y + 80 + 20 * i);

                spriteBatch.DrawString(_contentChest.ButtonFont, player.Name, position,
                    _networkManager.GetMyPlayer().Id.Equals(player.Id) ? Color.Green : Color.White);
            }
            
            _buttons.ForEach(b => b.Draw(spriteBatch));
            
            spriteBatch.End();
        }
    }
}