using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public class PinkGhostTargetPicker : IGhostTargetPicker
    {
        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            return player.Position + player.Direction * 4;
        }
    }
}