using System;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class PathNode
    {
        public Vector2 Position { get; }
        public PathEdge Up { get; private set; }
        public PathEdge Down { get; private set; }
        public PathEdge Left { get; private set; }
        public PathEdge Right { get; private set; }

        public PathNode(Vector2 pos)
        {
            Position = pos;
        }

        public void AddPortalTo(PathNode node, Direction dir)
        {
            var edge = new PathEdge(0, this, node, true);

            switch (dir)
            {
                case Direction.Up:
                    Up = edge;
                    break;
                case Direction.Down:
                    Down = edge;
                    break;
                case Direction.Left:
                    Left = edge;
                    break;
                case Direction.Right:
                    Right = edge;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

        public void AddEdgeTo(PathNode node)
        {
            var diff = Position - node.Position;
            var diffX = (int)diff.X;
            var diffY = (int) diff.Y;

            if (diffX != 0 && diffY != 0)
            {
                Console.WriteLine("Non perpendicular edges cannot be added");
                return;
            }

            if (diffX == 0 && diffY == 0)
            {
                Console.WriteLine("Attempted to add the same node as an edge");
                return;
            }

            if (diffX < 0)
            {
                Right = new PathEdge(Math.Abs(diffX), this, node);
                node.Left = new PathEdge(Math.Abs(diffX), node, this);
            } 
            else if (diffX > 0)
            {
                Left = new PathEdge(diffX, this, node);
                node.Right = new PathEdge(diffX, node, this);
            } else if (diffY < 0)
            {
                Down = new PathEdge(Math.Abs(diffY), this, node);
                node.Up = new PathEdge(Math.Abs(diffY), node, this);
            } else if (diffY > 0)
            {
                Up = new PathEdge(diffY, this, node);
                node.Down = new PathEdge(diffY, node, this);
            }
        }

        public bool At(Vector2 tilePos)
        {
            return Position == tilePos;
        }

        public override string ToString()
        {
            var s = $"{Position.X}:{Position.Y}";
            s += Up == null ? " " : "U";
            s += Down == null ? " " : "D";
            s += Left == null ? " " : "L";
            s += Right == null ? " " : "R";

            return s;

        }
    }
}