using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Pacman.Ghosts
{
    public class RedGhostTargetPicker : IGhostTargetPicker
    {

        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            return player.Position;
        }
    }
}