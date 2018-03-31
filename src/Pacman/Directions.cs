using Microsoft.Xna.Framework;

namespace Pacman
{
    public class Directions
    {
        public static readonly Vector2 Up = new Vector2(0, -1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Down = new Vector2(0, 1);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Stopped = Vector2.Zero;
    }
}