using System;
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

            var lastAnimation = Player.ActiveAnimation;
            
            _networkManager.Players.ForEach(p => p.Update(delta));
            var moving = false;
            if (InputManager.Player1Down.Down)
            {
                Player.ActiveAnimation = Player.Animation.WalkDown;
                Player.Position += new Vector2(0, Player.Speed * delta);
                moving = true;
            } else if (InputManager.Player1Up.Down)
            {
                
                Player.ActiveAnimation = Player.Animation.WalkUp;
                Player.Position -= new Vector2(0, Player.Speed * delta);
                moving = true;
            }
            
            
            if (InputManager.Player1Left.Down)
            {
                Player.ActiveAnimation = Player.Animation.WalkLeft;
                Player.Position -= new Vector2(Player.Speed * delta, 0);
                moving = true;
            } else if (InputManager.Player1Right.Down)
            {
                Player.ActiveAnimation = Player.Animation.WalkRight;
                Player.Position += new Vector2(Player.Speed * delta, 0);
                moving = true;
            }

            if (!moving)
            {
                Player.ActiveAnimation = Player.Animation.Idle;
            }
            else
            {
                if(lastAnimation != Player.ActiveAnimation)
                    _networkManager.SendPlayerAnimation(Player);
                _networkManager.SendPlayerPosition(Player);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            _networkManager.Players.ForEach(p => p.Draw(spriteBatch));
            
            spriteBatch.End();
        }
    }
}