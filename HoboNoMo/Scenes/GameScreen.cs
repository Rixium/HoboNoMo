using System;
using HoboNoMo.Helpers;
using HoboNoMo.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboNoMo.Scenes
{
    public class GameScreen : IScene
    {

        private readonly ContentChest _contentChest;
        private readonly NetworkManager _networkManager;
        private Camera _camera = new Camera(0, 0);
        private KeyboardState lastState;
        public Func<IScene, bool> RequestSceneChange { get; set; }

        public Player Player => _networkManager.GetMyPlayer();
        
        public GameScreen(ContentChest contentChest, NetworkManager networkManager)
        {
            _contentChest = contentChest;
            _networkManager = networkManager;

            Player.OnMove += OnPlayerMove;
            Player.OnOutfitChange += _networkManager.SendOutfitChange;
        }

        public void Update(float delta)
        {
            _networkManager.Update(delta);

            _camera.X = (int) Player.Position.X;
            _camera.Y = (int) Player.Position.Y;
            
            var lastAnimation = Player.ActiveAnimation;
            
            _networkManager.Players.ForEach(p => p.Update(delta));
            var moving = false;
            
            if (InputManager.Player1Down.Down)
            {
                Player.ActiveAnimation = Player.Animation.WalkDown;
                Player.YVelocity += delta * 30;
                moving = true;
            } else if (InputManager.Player1Up.Down)
            {
                
                Player.ActiveAnimation = Player.Animation.WalkUp;
                Player.YVelocity -= delta * 30;
                moving = true;
            } else if (InputManager.Player1Left.Down)
            {
                Player.ActiveAnimation = Player.Animation.WalkLeft;
                
                Player.XVelocity -= delta * 30;
                moving = true;
            } else if (InputManager.Player1Right.Down)
            {
             
                Player.ActiveAnimation = Player.Animation.WalkRight;
                
                Player.XVelocity += delta * 30;
                moving = true;
            }

            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.OemPlus) && lastState.IsKeyUp(Keys.OemPlus))
            {
                Player.NextOutfit();
            } else if (keyState.IsKeyDown(Keys.OemMinus) && lastState.IsKeyUp(Keys.OemMinus))
            {
                Player.LastOutfit();
            }

            lastState = keyState;
            
            if (!moving)
            {
                Player.ActiveAnimation = Player.Animation.Idle;
                if(lastAnimation != Player.ActiveAnimation)
                    _networkManager.SendPlayerAnimation(Player);
            }
            else
            {
                if(lastAnimation != Player.ActiveAnimation)
                    _networkManager.SendPlayerAnimation(Player);
                
            }
        }

        public void OnPlayerMove()
        {
            _networkManager.SendPlayerPosition(Player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.Get());

            _networkManager.Players.ForEach(p => p.Draw(spriteBatch));
            
            SpriteHelper.DrawRectangle(spriteBatch, new Rectangle(30, 30, 100, 100), Color.Blue);
            spriteBatch.End();
        }
    }
}