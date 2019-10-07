using System.Collections.Generic;
using HoboNoMo.Input;

namespace HoboNoMo
{
    public static class InputManager
    {
        public enum Binding
        {
            Player1Up,
            Player1Down,
            Player1Use
        }

        private static Dictionary<Binding, IInputBinding> _bindings = new Dictionary<Binding, IInputBinding>();

        public static IInputBinding Player1Down => _bindings[Binding.Player1Down];
        public static IInputBinding Player1Up => _bindings[Binding.Player1Up];
        public static IInputBinding Player1Use => _bindings[Binding.Player1Use];

        public static void AddBinding(Binding binding, IInputBinding inputBinding)
        {
            _bindings[binding] = inputBinding;
        }

        public static void Update(float deltaTime)
        {
            foreach (var input in _bindings)
            {
                input.Value.Update(deltaTime);
            }
        }
    }
}