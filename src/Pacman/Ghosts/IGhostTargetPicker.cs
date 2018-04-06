using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public interface IGhostTargetPicker
    {
        Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player);
    }
}