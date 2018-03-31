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

        public IEnumerable<PathEdge> Edges => _edges.Values;

        public PathEdge this[Vector2 direction]
        {
            get => _edges.TryGetValue(direction, out var edge) ? edge : null;
            private set => _edges[direction] = value;
        }

        public void AddEdgeTo(PathNode node, Vector2 direction)
        {
            var length = (int)Vector2.Distance(Position, node.Position);
            this[direction] = new PathEdge(length, this, node, direction);

            var reverse = Vector2.Negate(direction);
            node[reverse] = new PathEdge(length, node, this, reverse);
        }

        public void AddPortalTo(PathNode portalNode, Vector2 direction)
        {
            this[direction] = new PathEdge(0, this, portalNode,direction, true);

            var reverse = Vector2.Negate(direction);
            portalNode[reverse] = new PathEdge(0, portalNode, this, reverse, true);
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