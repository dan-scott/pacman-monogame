using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Pacman
{
    public class SpriteGrid
    {
        private const int TILE_SIZE = 20;

        private static readonly Vector2 TileSizeVector = new Vector2(TILE_SIZE);
        private static readonly Vector2 TileOffsetVector = new Vector2(TILE_SIZE / 2);

        private TextureAtlas _textureAtlas;
        private readonly List<string> _validCoords;

        private readonly List<(Vector2, string)> _tiles;

        public SpriteGrid(List<(Vector2, string)> tiles)
        {
            _validCoords = new List<string>();

            _tiles = tiles;
        }

        public void LoadContent()
        {
            var texture = GameServices.GetService<ContentManager>().Load<Texture2D>("level_tiles");
            _textureAtlas = new TextureAtlas("LevelTextures", texture);


            var cols = Enumerable.Range(0, 10).ToList();
            var rows = Enumerable.Range(0, 6).ToList();

            foreach (var col in cols)
            foreach (var row in rows)
            {
                var name = $"{(char) ('a' + col)}{row}";
                _validCoords.Add(name);
                _textureAtlas.CreateRegion(name, col * TILE_SIZE, row * TILE_SIZE, TILE_SIZE, TILE_SIZE);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Globals.DefaultTranslation);

            foreach (var (pos, tile) in _tiles.Where(x => _validCoords.Contains(x.Item2)))
            {
                var coord = pos * TileSizeVector - TileOffsetVector;
                spriteBatch.Draw(_textureAtlas[tile], coord, Color.White);
            }

            spriteBatch.End();
        }
    }
}