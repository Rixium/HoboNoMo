using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoboNoMo.Input
{
    public class GamePadThumbstickInputBinding : IInputBinding
    {
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public enum Thumbstick
        {
            Left,
            Right
        }

        private readonly PlayerIndex _index;
        private readonly Thumbstick _thumbstick;
        private readonly Direction _direction;

        public bool Held { get; set; }
        public bool Press { get; set; }
        public bool Down { get; set; }

        private GamePadState _lastState;
        private float _holdTime;

        public GamePadThumbstickInputBinding(PlayerIndex index, Thumbstick thumbstick, Direction direction)
        {
            _index = index;
            _thumbstick = thumbstick;
            _direction = direction;
        }

        public void Update(float delta)
        {
            var state = GamePad.GetState(_index);

            switch (_thumbstick)
            {
                case Thumbstick.Left:
                    ProcessLeft(delta, state);
                    break;
                case Thumbstick.Right:
                    ProcessRight(state);
                    break;
            }

            _lastState = state;
        }

        private void ProcessRight(GamePadState state)
        {
            throw new NotImplementedException();
        }

        private void ProcessLeft(float delta, GamePadState state)
        {
            switch (_direction)
            {
                case Direction.Left:
                    if (state.ThumbSticks.Left.X < -0.5f)
                    {
                        Down = true;
                        if (_lastState.ThumbSticks.Left.X < -0.5f)
                        {
                            _holdTime += delta;
                            if (_holdTime > 0.5f)
                            {
                                Held = true;
                            }
                        }
                        Press = _lastState.ThumbSticks.Left.X > -0.5f;
                    }
                    else
                    {
                        Down = false;
                        Held = false;
                        _holdTime = 0;
                    }

                    break;
                case Direction.Right:
                    if (state.ThumbSticks.Left.X > 0.5f)
                    {
                        Down = true;
                        if (_lastState.ThumbSticks.Left.X > 0.5f)
                        {
                            _holdTime += delta;
                            if (_holdTime > 0.5f)
                            {
                                Held = true;
                            }
                        }

                        Press = _lastState.ThumbSticks.Left.X < 0.5f;
                    }
                    else
                    {
                        Down = false;
                        Held = false;
                        _holdTime = 0;
                    }

                    break;
                case Direction.Down:
                    if (state.ThumbSticks.Left.Y < -0.5f)
                    {
                        Down = true;
                        if (_lastState.ThumbSticks.Left.Y < -0.5f)
                        {
                            _holdTime += delta;
                            if (_holdTime > 0.5f)
                            {
                                Held = true;
                            }
                        }

                        Press = _lastState.ThumbSticks.Left.Y > -0.5f;
                    }
                    else
                    {
                        Down = false;
                        Held = false;
                        _holdTime = 0;
                    }

                    break;
                case Direction.Up:
                    if (state.ThumbSticks.Left.Y > 0.5f)
                    {
                        Down = true;
                        if (_lastState.ThumbSticks.Left.Y > 0.5f)
                        {
                            _holdTime += delta;
                            if (_holdTime > 0.5f)
                            {
                                Held = true;
                            }
                        }

                        Press = _lastState.ThumbSticks.Left.Y < 0.5f;
                    }
                    else
                    {
                        Down = false;
                        Held = false;
                        _holdTime = 0;
                    }

                    break;
            }
        }
    }
}