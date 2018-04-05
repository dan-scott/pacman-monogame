using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public class PinkGhostDirector : IGhostDirector
    {
        private readonly LevelGraph _graph;
        public Vector2 StartPos { get; }

        public PinkGhostDirector(LevelGraph graph)
        {
            _graph = graph;
            StartPos = new Vector2(14, 11);
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
}