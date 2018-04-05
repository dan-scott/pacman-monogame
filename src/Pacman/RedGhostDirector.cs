using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class RedGhostDirector : IGhostDirector
    {
        public RedGhostDirector(TileGrid allTiles)
        {
            StartPos = allTiles.Find(LevelTile.GhostSpawn).First();
        }

        public Vector2 StartPos { get; }

        public Vector2 GetTargetNode(GameTime time, Ghost ghost, Player player)
        {
            return player.NextNode;
        }
    }
}
