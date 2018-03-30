using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class LevelNavigator
    {
        private readonly List<PathNode> _nodes;

        public IEnumerable<PathNode> Nodes => _nodes;

        private static readonly LevelTile[] PathTileTypes = {LevelTile.Path, LevelTile.Dot, LevelTile.PowerPellet, LevelTile.Start};

        public LevelNavigator(TileGrid grid)
        {
            var pathGrid = grid.Filter(PathTileTypes);

            _nodes = GenerateNodes(pathGrid);
        }

        private List<PathNode> GenerateNodes(TileGrid grid)
        {
            var nodes = new List<PathNode>();

            var nodeStack = new Stack<(Vector2 dir, PathNode node)>();

            var startNode = new PathNode(grid.Find(LevelTile.Start).First());

            nodes.Add(startNode);

            PushToStack(startNode, Vector2.Zero);

            while (nodeStack.Any())
            {
                var (dir, node) = nodeStack.Pop();

                if (node[dir] != null)
                {
                    continue;
                }

                var tilePos = node.Position;

                do
                {
                    tilePos += dir;
                } while (IsEdgeTile(tilePos, dir));

                if (nodes.Any(x => x.At(tilePos)))
                {
                    continue;
                }

                var nextNode = new PathNode(tilePos);

                nodes.Add(nextNode);

                node.AddEdgeTo(nextNode, dir);

                PushToStack(nextNode, dir);
            }

            return nodes;

            void PushToStack(PathNode node, Vector2 currentDir)
            {
                var reverse = Vector2.Negate(currentDir);
                grid
                    .GetAdjacent(node.Position)
                    .Where(x => x.tile != LevelTile.None && x.dir != reverse)
                    .ToList()
                    .ForEach(x => nodeStack.Push((x.dir, node)));
            }

            bool IsEdgeTile(Vector2 tilePos, Vector2 dir)
            {
                var nextPos = tilePos + dir;

                if (grid[nextPos] == LevelTile.None)
                {
                    return false;
                }

                var perp = new Vector2(dir.Y, dir.X);
                var perpNeg = Vector2.Negate(perp);

                return grid[tilePos + perp] == LevelTile.None && grid[tilePos + perpNeg] == LevelTile.None;
            }
        }

        
    }
}