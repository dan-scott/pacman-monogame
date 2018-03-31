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
            var bounds = grid.GetBounds();

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

                bool isEdgeTile;
                bool isPortalTile;
                bool reachedExistingTile;
                do
                {
                    tilePos += dir;
                    reachedExistingTile = nodes.Any(x => x.At(tilePos));
                    isPortalTile = !reachedExistingTile && IsPortalTile(tilePos, dir);
                    isEdgeTile = !reachedExistingTile && !isPortalTile && IsEdgeTile(tilePos, dir);
                } while (!reachedExistingTile && !isPortalTile && isEdgeTile);

                var nextNode = nodes.FirstOrDefault(x => x.At(tilePos));

                if (nextNode == null)
                {
                    nextNode = new PathNode(tilePos);
                    nodes.Add(nextNode);
                }

                node.AddEdgeTo(nextNode, dir);

                PushToStack(nextNode, dir);

                if (isPortalTile)
                {
                    var portalTile = tilePos + dir;

                    if (portalTile.X < bounds.Left)
                    {
                        portalTile.X = bounds.Right - 1;
                    } 
                    else if (portalTile.X >= bounds.Right)
                    {
                        portalTile.X = bounds.Left;
                    }
                    else if (portalTile.Y < bounds.Top)
                    {
                        portalTile.Y = bounds.Bottom - 1;
                    } 
                    else if (portalTile.Y >= bounds.Bottom)
                    {
                        portalTile.Y = bounds.Top;
                    }

                    var portalNode = new PathNode(portalTile);

                    nextNode.AddPortalTo(portalNode, dir);

                    nodes.Add(portalNode);

                    PushToStack(portalNode, dir);
                }
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

            bool IsPortalTile(Vector2 pos, Vector2 dir)
            {
                var nextPos = pos + dir;
                return bounds.Contains(pos) && !bounds.Contains(nextPos);
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