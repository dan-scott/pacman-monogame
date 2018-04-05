using Microsoft.Xna.Framework;
using Pacman.Ghosts;

namespace Pacman
{
    public interface IGhostDirector
    {
        Vector2 StartPos { get; }

        Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player);
    }
}