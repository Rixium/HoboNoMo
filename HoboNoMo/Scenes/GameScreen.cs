using System;
using HoboNoMo.Helpers;
using HoboNoMo.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo.Scenes
{
    public class GameScreen : IScene
    {
        private readonly ContentChest _contentChest;
        private readonly NetworkManager _networkManager;
        public Func<IScene, bool> RequestSceneChange { get; set; }

        public Player Player => _networkManager.GetMyPlayer();
        
        public GameScreen(ContentChest contentChest, NetworkManager networkManager)
        {
            _contentChest = contentChest;
            _networkManager = networkManager;
        }

        public void Update(float delta)
        {
            _networkManager.Update(delta);
            
            if (InputManager.Player1Down.Held)
            {
                Player.Position += new Vector2(0, 50 * delta);
                _networkManager.SendPlayerPosition(Player);
            } else if (InputManager.Player1Up.Held)
            {
                Player.Position -= new Vector2(0, 50 * delta);
                _networkManager.SendPlayerPosition(Player);
            }
            
            
            if (InputManager.Player1Left.Held)
            {
                Player.Position -= new Vector2(50 * delta, 0);
                _networkManager.SendPlayerPosition(Player);
            } else if (InputManager.Player1Right.Held)
            {
                Player.Position += new Vector2(50 * delta, 0);
                _networkManager.SendPlayerPosition(Player);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            foreach (var player in _networkManager.Players)
            {
                var nameMeasurements = StringHelpers.Measure(player.Name, _contentChest.ButtonFont);
                spriteBatch.DrawString(_contentChest.ButtonFont, player.Name, player.Position - new Vector2(nameMeasurements.X / 2 + 16, 5 + nameMeasurements.Y), Color.White);
                SpriteHelper.DrawRectangle(spriteBatch, new Rectangle((int) player.Position.X, (int) player.Position.Y, 32, 32), Color.White);
            }
            spriteBatch.End();
        }
    }
}