using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class LevelGraph
    {
        private readonly Dictionary<Vector2, List<Vector2>> _nodes;

        private static readonly LevelTile[] PathTileTypes =
        {
            LevelTile.Path,
            LevelTile.Dot,
            LevelTile.PowerPellet,
            LevelTile.Start,
        };

        private readonly TileGrid _pathGrid;
        private Vector2 _bottomRight;
        private Vector2 _topLeft;

        public IEnumerable<Vector2> Nodes => _nodes.Keys;

        public bool ContainsNode(Vector2 node) => _nodes.ContainsKey(node);

        public LevelGraph(TileGrid grid)
        {
            _pathGrid = grid.Filter(PathTileTypes);

            var gridPositions = grid.AsEnumerable().Select(x => x.Item1).ToList();

            _topLeft = new Vector2(gridPositions.Min(x => x.X), gridPositions.Min(x => x.Y));
            _bottomRight = new Vector2(gridPositions.Max(x => x.X), gridPositions.Max(x => x.Y));

            var positions = _pathGrid.AsEnumerable().Select(x => x.Item1).ToList();

            var byX = positions.ToLookup(pos => pos.X);
            var byY = positions.ToLookup(pos => pos.Y);

            _nodes = positions.ToDictionary(
                pos => pos,
                pos =>
                {
                    var a = byX[pos.X].Where(x => Vector2.Distance(x, pos) == 1);
                    var b = byY[pos.Y].Where(x => Vector2.Distance(x, pos) == 1);
                    return a.Concat(b).ToList();
                });
        }

        public IEnumerable<Vector2> Adjacent(Vector2 node) => _nodes[node];

        public bool HasAdjacent(Vector2 pos, Vector2 direction)
        {
            var next = pos + direction;
            return _nodes[pos].Any(x => x == next);
        }

        public bool IsPortal(Vector2 pos, Vector2 direction)
            => direction == Directions.Left && pos.X == _topLeft.X ||
               direction == Directions.Right && pos.X == _bottomRight.X ||
               direction == Directions.Up && pos.Y == _topLeft.Y ||
               direction == Directions.Down && pos.Y == _topLeft.Y;

        public Vector2 PortalTo(Vector2 pos, Vector2 direction)
        {
            if (direction == Directions.Left)
            {
                return new Vector2(_bottomRight.X, pos.Y);
            }

            if (direction == Directions.Right)
            {
                return new Vector2(_topLeft.X, pos.Y);
            }

            return direction == Directions.Up 
                ? new Vector2(pos.X, _bottomRight.Y) 
                : new Vector2(pos.X, _topLeft.Y);
        }

        public Vector2 Closest(Vector2 target)
        {
            return _nodes.Keys.OrderBy(node => Vector2.Distance(target, node)).First();
        }
    }
}