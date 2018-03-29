using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class Level
    {

        private const int TILE_SIZE = 20;
        private const int OFFSET_X = 120;
        private const int OFFSET_Y = 100;

        public int Width { get; private set; }
        public int Height { get; private set; }
        private LevelTile[,] _tiles;
        private readonly Texture2D _tile;
        
        public Level(GraphicsDevice device)
        {
            ParseMaze(DEFAULT_MAZE);
            _tile = new Texture2D(device, TILE_SIZE, TILE_SIZE);
            var tileColors = Enumerable.Range(1, TILE_SIZE * TILE_SIZE).Select(_ => Color.White).ToArray();
            _tile.SetData(tileColors);

        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    spriteBatch.Draw(_tile, new Vector2(x * TILE_SIZE + OFFSET_X, y * TILE_SIZE + OFFSET_Y), TileColors[_tiles[y,x]]);
                }
            }
        }

        private void ParseMaze(string maze)
        {
            var lines = maze.Trim().Split('\n');
            
            Height = lines.Length;
            Width = lines[0].Length;
            
            if (lines[0].EndsWith("\r"))
            {
                Width--;
            }

            _tiles = new LevelTile[Height,Width];
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _tiles[y, x] = Tile(lines[y][x]);
                }
            }

        }

        private static LevelTile Tile(char c)
        {
            switch (c)
            {
                case '0':
                    return LevelTile.Wall;
                
                case '*':
                    return LevelTile.Dot;
                
                case '+':
                    return LevelTile.PowerPellet;
                
                case '#':
                    return LevelTile.MonsterWall;
                
                default:
                    return LevelTile.Empty;
            }
        }

        private static readonly Dictionary<LevelTile, Color> TileColors = new Dictionary<LevelTile, Color>
        {
            [LevelTile.Dot] = Color.DarkGray,
            [LevelTile.Empty] = Color.Black,
            [LevelTile.MonsterWall] = Color.Blue,
            [LevelTile.PowerPellet] = Color.Yellow,
            [LevelTile.Wall] = Color.Blue,
        };


        private const string DEFAULT_MAZE = @"
0000000000000000000000000000
0************00************0
0*0000*00000*00*00000*0000*0
0+0  0*0   0*00*0   0*0  0+0
0*0000*00000*00*00000*0000*0
0**************************0
0*0000*00*00000000*00*0000*0
0*0000*00*00000000*00*0000*0
0******00****00****00******0
000000*00000 00 00000*000000
     0*00          00*0     
     0*00 ######## 00*0     
     0*00 #      # 00*0     
000000*00 #      # 00*000000
      *   #      #   *      
000000*00 #      # 00*000000
     0*00 ######## 00*0     
     0*00          00*0     
     0*00 00000000 00*0     
000000*00 00000000 00*000000
0************00************0
0*0000*00000*00*00000*0000*0
0*0000*00000*00*00000*0000*0
0+**00****************00**+0
000*00*00*00000000*00*00*000
000*00*00*00000000*00*00*000
0******00****00****00******0
0*0000000000*00*0000000000*0
0*0000000000*00*0000000000*0
0**************************0
0000000000000000000000000000
";
    }

    internal enum LevelTile
    {
        Empty,
        Dot,
        PowerPellet,
        Wall,
        MonsterWall,
    }
}