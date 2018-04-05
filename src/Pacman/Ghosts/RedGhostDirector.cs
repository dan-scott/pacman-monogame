using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public class RedGhostDirector : IGhostDirector
    {
        public RedGhostDirector()
        {
            StartPos = new Vector2(13, 11);
        }

        public Vector2 StartPos { get; }

        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            return player.NextNode;
        }
    }
}
