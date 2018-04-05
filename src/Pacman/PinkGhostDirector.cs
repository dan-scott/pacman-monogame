using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class PinkGhostDirector : IGhostDirector
    {
        private readonly LevelGraph _graph;
        public Vector2 StartPos { get; }

        public PinkGhostDirector(TileGrid allTiles, LevelGraph graph)
        {
            _graph = graph;
            StartPos = allTiles.Find(LevelTile.GhostSpawn).Skip(1).First();
        }

        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            var targetNode = player.NextNode;
            var playerDir = player.Direction;

            for (var i = 0; i < 3; i++)
            {
                var next = targetNode + playerDir;
                if (_graph.ContainsNode(next))
                {
                    targetNode = next;
                }
            }

            return targetNode;
        }
    }

    public class BlueGhostDirector : IGhostDirector
    {
        private readonly LevelGraph _graph;
        public Vector2 StartPos { get; }

        public BlueGhostDirector(TileGrid allTiles, LevelGraph graph)
        {
            _graph = graph;
            StartPos = allTiles.Find(LevelTile.GhostSpawn).Skip(1).First();
        }

        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            var targetNode = player.NextNode;
            var playerDir = player.Direction;
            var ghostNode = ghost.NextNode;

            for (var i = 0; i < 1; i++)
            {
                var next = targetNode + playerDir;
                if (_graph.ContainsNode(next))
                {
                    targetNode = next;
                }
            }

            var target = targetNode + (targetNode - ghostNode);

            return _graph.Closest(target);
        }
    }
}