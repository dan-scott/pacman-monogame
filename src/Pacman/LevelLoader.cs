using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class LevelLoader
    {

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

        public SpriteGrid Sprites { get; private set; }
        public TileGrid AllTiles { get; private set; }
        public TileGrid Dots { get; private set; }
        public LevelNavigator Navigator { get; private set; }


        private LevelLoader()
        {
        }

        public static LevelLoader LoadDefaultLevel()
        {
            var loader = new LevelLoader();

            loader.Init();

            return loader;
        }

        private void Init()
        {
            var tileCodes = LoadLevelTileCodes();

            AllTiles = new TileGrid(tileCodes);

            Sprites = new SpriteGrid(tileCodes);

            Dots = AllTiles.Filter(LevelTile.PowerPellet, LevelTile.Dot);

            Navigator = new LevelNavigator(AllTiles);
        }



        private List<(Vector2, string)> LoadLevelTileCodes()
        {
            const string path = "Content\\level.txt";

            var lines = File.ReadAllLines(path);

            var tiles = new List<(Vector2, string)>();

            for (var row = 0; row < lines.Length; row++)
            {
                var columns = lines[row].Split(',').Where(x => string.IsNullOrEmpty(x) == false).ToArray();
                for (var col = 0; col < columns.Length; col++)
                {
                    var pos = new Vector2(col, row);
                    tiles.Add((pos, columns[col]));
                }
            }

            return tiles;
        }


    }
}