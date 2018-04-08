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
        private readonly Vector2 _tileSizeVector;
        private readonly Player _player;
        private readonly SpriteGrid _spriteGrid;
        private readonly List<Ghost> _ghosts;
        private readonly LevelDots _dots;

        public GameLevel(int tileSideSize)
        {
            var loader = LevelLoader.LoadDefaultLevel();

            _tileSizeVector = new Vector2(tileSideSize);

            var graph = loader.Graph;

            _player = new Player(graph);

            _player.Reset(loader.AllTiles.Find(LevelTile.Start).First());

            _spriteGrid = loader.Sprites;

            _dots = new LevelDots(loader.AllTiles);

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

            _dots.Draw(spriteBatch, _tileSizeVector);

            _player.Draw(spriteBatch, _tileSizeVector);

            _ghosts.ForEach(ghost => ghost.Draw(spriteBatch, _tileSizeVector));
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _dots.Update(_player);
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