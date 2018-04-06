using Microsoft.Xna.Framework;

namespace Pacman.Ghosts
{
    public class OrangeGhostTargetPicker : IGhostTargetPicker
    {
        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            var distance = Vector2.Distance(ghost.Position, player.Position);

            return distance > 8
                ? player.Position
                : new Vector2(0, 33);
        }
    }
}