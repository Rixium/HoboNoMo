using System;
using Microsoft.Xna.Framework;

namespace HoboNoMo
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public int Cash { get; set; }
        public Vector2 Position { get; set; }
        
    }
}