using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Pacman.Ghosts;

namespace Pacman
{
    public class GameLevel
    {
        private readonly TileGrid _dots;
        private readonly int _tileSideSize;
        private readonly Vector2 _tileSizeVector;
        private readonly Player _player;
        private readonly SpriteGrid _spriteGrid;
        private readonly List<Ghost> _ghosts;

        public GameLevel(int tileSideSize)
        {
            var loader = LevelLoader.LoadDefaultLevel();

            _tileSideSize = tileSideSize;
            _tileSizeVector = new Vector2(tileSideSize);
            _dots = loader.Dots;
            var graph = loader.Graph;
            _player = new Player(graph);

            _player.Reset(loader.AllTiles.Find(LevelTile.Start).First());

            _spriteGrid = loader.Sprites;

            _ghosts = new List<Ghost>
            {
                Ghost.Red(graph),
                Ghost.Blue(graph),
                Ghost.Pink(graph),
                Ghost.Orange(graph),
            };

            _ghosts.ForEach(ghost => ghost.Reset());
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _spriteGrid.Draw(spriteBatch);

            DrawDots(spriteBatch);

            _player.Draw(spriteBatch, _tileSizeVector);

            _ghosts.ForEach(ghost => ghost.Draw(spriteBatch, _tileSizeVector));
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
            _ghosts.ForEach(ghost => ghost.Update(gameTime, _player));
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