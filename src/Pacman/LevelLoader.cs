using System.Collections.Generic;

namespace Pacman
{
    public class LevelLoader
    {
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
     0*00 ###--### 00*0     
     0*00 #||||||# 00*0     
000000*00 #|    |# 00*000000
      *   #|    |#   *      
000000*00 #||||||# 00*000000
     0*00 ######## 00*0     
     0*00          00*0     
     0*00 00000000 00*0     
000000*00 00000000 00*000000
0************00************0
0*0000*00000*00*00000*0000*0
0*0000*00000*00*00000*0000*0
0+**00*******@********00**+0
000*00*00*00000000*00*00*000
000*00*00*00000000*00*00*000
0******00****00****00******0
0*0000000000*00*0000000000*0
0*0000000000*00*0000000000*0
0**************************0
0000000000000000000000000000
";

        private static readonly Dictionary<char,LevelTile> TileMap = new Dictionary<char, LevelTile>
        {
            [' '] = LevelTile.Path,
            ['*'] = LevelTile.Dot,
            ['-'] = LevelTile.MonsterEntrance,
            ['|'] = LevelTile.MonsterSpawn,
            ['#'] = LevelTile.MonsterWall,
            ['+'] = LevelTile.PowerPellet,
            ['@'] = LevelTile.Start,
            ['0'] = LevelTile.Wall,
        };

        
        public int Width { get; set; }

        public int Height { get; set; }

        public TileGrid AllTiles { get; private set; }
        public TileGrid Walls { get; private set; }
        public TileGrid Dots { get; private set; }
        public LevelNavigator Navigator { get; private set; }


        private LevelLoader()
        {
        }

        public static LevelLoader LoadDefaultLevel()
        {
            var loader = new LevelLoader();

            loader.Init(DEFAULT_MAZE);

            return loader;
        }

        private void Init(string defaultMaze)
        {
            Parse(defaultMaze);

            Walls = AllTiles.Filter(LevelTile.MonsterEntrance, LevelTile.MonsterWall, LevelTile.Wall);
            Dots = AllTiles.Filter(LevelTile.PowerPellet, LevelTile.Dot);

            Navigator = new LevelNavigator(AllTiles);
        }



        private void Parse(string maze)
        {
            var lines = maze.Trim().Split('\n');

            Height = lines.Length;
            Width = lines[0].Length;

            if (lines[0].EndsWith("\r"))
            {
                Width--;
            }

            AllTiles = new TileGrid();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    AllTiles[x, y] = ToTile(lines[y][x]);
                }
            }
        }

        private static LevelTile ToTile(char c)
            => TileMap.TryGetValue(c, out var tile) ? tile : LevelTile.None;
    }
}