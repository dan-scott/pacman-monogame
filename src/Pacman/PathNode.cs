using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class PathNode
    {
        public Vector2 Position { get; }

        private readonly Dictionary<Vector2, PathEdge> _edges;

        public PathNode(Vector2 pos)
        {
            Position = pos;
            _edges = new Dictionary<Vector2, PathEdge>();
        }

        public PathEdge this[Vector2 direction]
        {
            get => _edges.TryGetValue(direction, out var edge) ? edge : null;
            private set => _edges[direction] = value;
        }

        public void AddEdgeTo(PathNode node, Vector2 direction)
        {
            var diff = Position - node.Position;
            var length = (int)Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y));
            this[direction] = new PathEdge(length, this, node);
            node[Vector2.Negate(direction)] = new PathEdge(length, node, this);
        }

        public bool At(Vector2 tilePos)
        {
            return Position == tilePos;
        }

        public override string ToString()
        {
            var s = Position + " - ";
            s += string.Join(";", _edges.Values.Select(e => e.End.Position.ToString()));
            return s;

        }
    }
}