using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public class BlueGhostDirector : IGhostDirector
    {
        private readonly LevelGraph _graph;
        public Vector2 StartPos { get; }

        public BlueGhostDirector(LevelGraph graph)
        {
            _graph = graph;
            StartPos = new Vector2(12, 11);
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