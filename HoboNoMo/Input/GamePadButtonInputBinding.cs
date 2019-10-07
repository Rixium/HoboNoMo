using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoboNoMo.Input
{
    public class GamePadButtonInputBinding : IInputBinding
    {
        private readonly PlayerIndex _index;
        private readonly Buttons _button;

        private float _holdTime;
        private GamePadState _lastState;

        public GamePadButtonInputBinding(PlayerIndex index, Buttons button)
        {
            _index = index;
            _button = button;
        }

        public bool Held { get; set; }
        public bool Press { get; set; }

        public void Update(float delta)
        {
            var state = GamePad.GetState(_index);

            Press = state.IsButtonDown(_button) && _lastState.IsButtonUp(_button);

            if (state.IsButtonDown(_button) && _lastState.IsButtonDown(_button))
            {
                _holdTime += delta;

                if (_holdTime >= 0.5f)
                {
                    Held = true;
                }
            }

            if (state.IsButtonUp(_button))
            {
                Held = false;
                _holdTime = 0;
            }

            _lastState = state;
        }
    }
}