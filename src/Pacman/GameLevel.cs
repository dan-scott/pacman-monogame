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
            [LevelTile.MonsterEntrance] = Color.Blue,
            [LevelTile.MonsterSpawn] = Color.Black,
            [LevelTile.MonsterWall] = Color.Blue,
            [LevelTile.None] = Color.Black,
            [LevelTile.PowerPellet] = Color.Orange,
            [LevelTile.Start] = Color.Red,
            [LevelTile.Wall] = Color.Blue,
        };

        private readonly TileGrid _dots;
        private readonly int _tileSideSize;
        private readonly LevelNavigator _navigator;
        private readonly Vector2 _tileSizeVector;
        private readonly Size2 _tileSize;
        private readonly Player _player;
        private readonly SpriteGrid _spriteGrid;

        public GameLevel(int tileSideSize)
        {
            var loader = LevelLoader.LoadDefaultLevel();

            _tileSideSize = tileSideSize;
            _tileSize = new Size2(tileSideSize, tileSideSize);
            _tileSizeVector = new Vector2(tileSideSize);
            _dots = loader.Dots;
            _navigator = loader.Navigator;
            _player = new Player();
            var start = _navigator.Nodes.First();
            _player.Reset(start.Position, start.Edges.First());

            _spriteGrid = loader.Sprites;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _spriteGrid.Draw(spriteBatch);


            foreach (var (position, tile) in _dots.ToScreenSpace(_tileSideSize))
            {
                var radius = tile == LevelTile.Dot ? 4 : 10;
                spriteBatch.DrawCircle(position + _tileSizeVector / 2, radius, 10, Color.Yellow, radius);
            }

            foreach (var navigatorNode in _navigator.Nodes)
            {
                spriteBatch.DrawCircle(navigatorNode.Position * _tileSizeVector, 5, 10, Color.Red);
                foreach (var edge in navigatorNode.Edges.Where(x => !x.IsPortal))
                {
                    var start = edge.Start.Position * _tileSizeVector;
                    var end = edge.End.Position * _tileSizeVector;
                    spriteBatch.DrawLine(start, end, Color.Green, 2);
                }
            }

            _player.Draw(spriteBatch, _tileSizeVector);

        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
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