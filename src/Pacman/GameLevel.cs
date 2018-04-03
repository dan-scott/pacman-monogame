using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Pacman
{
    public class GameLevel
    {
        private static readonly Dictionary<LevelTile, Color> TileColors = new Dictionary<LevelTile, Color>
        {
            [LevelTile.Path] = Color.Black,
            [LevelTile.Dot] = Color.DarkGray,
            [LevelTile.GhostEntrance] = Color.Blue,
            [LevelTile.GhostSpawn] = Color.Black,
            [LevelTile.GhostWall] = Color.Blue,
            [LevelTile.None] = Color.Black,
            [LevelTile.PowerPellet] = Color.Orange,
            [LevelTile.Start] = Color.Red,
            [LevelTile.Wall] = Color.Blue,
        };

        private readonly TileGrid _dots;
        private readonly int _tileSideSize;
        private readonly Vector2 _tileSizeVector;
        private readonly Player _player;
        private readonly SpriteGrid _spriteGrid;
        private readonly LevelGraph _graph;
        private Ghost _ghost;

        public GameLevel(int tileSideSize)
        {
            var loader = LevelLoader.LoadDefaultLevel();

            _tileSideSize = tileSideSize;
            _tileSizeVector = new Vector2(tileSideSize);
            _dots = loader.Dots;
            _graph = loader.Graph;
            _player = new Player(_graph);
            
            _player.Reset(loader.AllTiles.Find(LevelTile.Start).First());

            _spriteGrid = loader.Sprites;

            _ghost = new Ghost(_graph);

            _ghost.Reset(loader.AllTiles.Find(LevelTile.GhostSpawn).First());

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _spriteGrid.Draw(spriteBatch);

            DrawDots(spriteBatch);

            _player.Draw(spriteBatch, _tileSizeVector);

            _ghost.Draw(spriteBatch, _tileSizeVector);

        }


        private void DrawDots(SpriteBatch spriteBatch)
        {
            foreach (var (position, tile) in _dots.ToScreenSpace(_tileSideSize))
            {
                var radius = tile == LevelTile.Dot ? 4 : 10;
                spriteBatch.DrawCircle(position + _tileSizeVector / 2, radius, 10, Color.Yellow, radius);
            }
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _ghost.Update(gameTime, _player);
        }

        public void LoadContent()
        {
            _player.LoadContent();
            _spriteGrid.LoadContent();
        }

        public void UnloadContent()
        {
            _player.UnloadContent();
        }
    }
}