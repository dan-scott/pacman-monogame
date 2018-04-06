using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public class BlueGhostTargetPicker : IGhostTargetPicker
    {
        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            var targetNode = player.Position + player.Direction * 2;

            targetNode += targetNode - ghost.Position;

            return targetNode;
        }
    }
}