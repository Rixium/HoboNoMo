using System;
using HoboNoMo.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboNoMo
{
    public class Player
    {
        
        public enum Animation
        {
            Idle,
            WalkDown,
            WalkLeft,
            WalkRight,
            WalkUp
        }

        public ContentChest.Outfit Outfit = ContentChest.Outfit.Nude;

        private Animation _activeAnimation = Animation.Idle;
        public Animation ActiveAnimation
        {
            get => _activeAnimation;
            set
            {
                if (value == _activeAnimation) return;
                _activeAnimation = value;
                _frameCounter = 0;
                _animationFrame = 0;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public int Cash { get; set; }
        public Vector2 Position { get; set; }
        
        public float XVelocity { get; set; } = 0.0f;
        public float YVelocity { get; set; } = 0.0f;
        public float MaxVelocity { get; set; } = 500.0f;
        public Action OnMove { get; set; }

        private int _animationFrame;
        private float _frameCounter;

        public void Update(float delta)
        {
            _frameCounter += delta;
            
            if (_frameCounter > 0.2f)
            {
                _animationFrame++;
                _frameCounter = 0;
            }

            if (_animationFrame == Game1.ContentChest.PlayerTextures[Outfit][_activeAnimation].Length)
            {
                _animationFrame = 0;
            }

            var newPosition = new Vector2(Position.X, Position.Y);

            XVelocity = MathHelper.Clamp(XVelocity, -MaxVelocity, MaxVelocity);
            YVelocity = MathHelper.Clamp(YVelocity, -MaxVelocity, MaxVelocity);
            
            newPosition.X += XVelocity;
            newPosition.Y += YVelocity;

            XVelocity *= 0.9f;
            YVelocity *= 0.9f;
            
            if(XVelocity < 0.2f && XVelocity > -0.2f)
                XVelocity = 0;
            if (YVelocity < 0.2f && YVelocity > -0.2f)
                YVelocity = 0;
             
            if (Math.Abs(newPosition.X - Position.X) > 0.1f || Math.Abs(newPosition.Y - Position.Y) > 0.1f)
            {
                Position = newPosition;
                OnMove?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var nameMeasurements = StringHelpers.Measure(Name, Game1.ContentChest.ButtonFont);
            var width = Game1.ContentChest.PlayerTextures[Outfit][ActiveAnimation][_animationFrame].Width;
            var textPosition = Position;
            textPosition.X += width / 2.0f;
            textPosition.X -= nameMeasurements.X / 2;
            textPosition.Y -= nameMeasurements.Y + 10;
            spriteBatch.DrawString(Game1.ContentChest.ButtonFont, Name, textPosition, Color.White);
            spriteBatch.Draw(Game1.ContentChest.PlayerTextures[Outfit][ActiveAnimation][_animationFrame], Position, Color.White);
        }
        

    }
}