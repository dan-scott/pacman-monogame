using System.Collections.Generic;
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
            [LevelTile.Dot] = Color.Yellow,
            [LevelTile.MonsterEntrance] = Color.Blue,
            [LevelTile.MonsterSpawn] = Color.Black,
            [LevelTile.MonsterWall] = Color.Blue,
            [LevelTile.None] = Color.Black,
            [LevelTile.PowerPellet] = Color.Orange,
            [LevelTile.Start] = Color.Red,
            [LevelTile.Wall] = Color.Blue,
        };

        private readonly TileGrid _walls;
        private readonly TileGrid _dots;
        private readonly int _tileSize;
        private readonly LevelNavigator _navigator;

        public GameLevel(int tileSize)
        {
            var loader = LevelLoader.LoadDefaultLevel();

            _tileSize = tileSize;
            _walls = loader.Walls;
            _dots = loader.Dots;
            _navigator = loader.Navigator;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var (position, tile) in _walls.ToScreenSpace(_tileSize))
            {
                spriteBatch.DrawRectangle(position, new Size2(_tileSize, _tileSize), TileColors[tile], _tileSize);
            }

            foreach (var (position, tile) in _dots.ToScreenSpace(_tileSize))
            {
                spriteBatch.DrawRectangle(position, new Size2(_tileSize, _tileSize), TileColors[tile], _tileSize);
            }

            foreach (var navigatorNode in _navigator.Nodes)
            {
                spriteBatch.DrawCircle(navigatorNode.Position * new Vector2(_tileSize), 5, 10, Color.Red, 5);
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }

}