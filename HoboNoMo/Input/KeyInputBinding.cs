using Microsoft.Xna.Framework.Input;

namespace HoboNoMo.Input
{
    public class KeyInputBinding : IInputBinding
    {
        private KeyboardState _lastState;
        public bool Held { get; set; }
        public bool Press { get; set; }
        public bool Down { get; set; }

        private float _lastTimer;

        private readonly Keys _key;

        public KeyInputBinding(Keys key)
        {
            _key = key;
        }

        public void Update(float delta)
        {
            var keyState = Keyboard.GetState();

            Press = keyState.IsKeyDown(_key) && _lastState.IsKeyUp(_key);
            Down = keyState.IsKeyDown(_key);
            
            if (keyState.IsKeyDown(_key) && !_lastState.IsKeyUp(_key))
                _lastTimer += delta;
            else
            {
                _lastTimer = 0;
                Held = false;
            }

            Held = _lastTimer >= 0.5f;

            _lastState = keyState;
        }
    }
}