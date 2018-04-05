using Microsoft.Xna.Framework;

namespace Pacman
{
    public interface IGhostDirector
    {
        Vector2 StartPos { get; }

        Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player);
    }
}