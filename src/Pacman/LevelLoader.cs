using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class LevelLoader
    {
        
        public int Width { get; set; }

        public int Height { get; set; }

        public SpriteGrid Sprites { get; private set; }
        public TileGrid AllTiles { get; private set; }
        public LevelGraph Graph { get; private set; }


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

            Graph = new LevelGraph(AllTiles);
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