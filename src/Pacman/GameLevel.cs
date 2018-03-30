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
        private readonly int _tileSideSize;
        private readonly LevelNavigator _navigator;
        private readonly Vector2 _tileSizeVector;
        private readonly Size2 _tileSize;

        public GameLevel(int tileSideSize)
        {
            var loader = LevelLoader.LoadDefaultLevel();

            _tileSideSize = tileSideSize;
            _tileSize = new Size2(tileSideSize, tileSideSize);
            _tileSizeVector = new Vector2(tileSideSize);
            _walls = loader.Walls;
            _dots = loader.Dots;
            _navigator = loader.Navigator;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var (position, tile) in _walls.ToScreenSpace(_tileSideSize))
            {
                spriteBatch.DrawRectangle(position, _tileSize, TileColors[tile], _tileSideSize);
            }

            foreach (var (position, tile) in _dots.ToScreenSpace(_tileSideSize))
            {
                spriteBatch.DrawRectangle(position, _tileSize, TileColors[tile], _tileSideSize);
            }

            foreach (var navigatorNode in _navigator.Nodes)
            {
                spriteBatch.DrawCircle(navigatorNode.Position * _tileSizeVector, 5, 10, Color.Red, 5);
                foreach (var edge in navigatorNode.Edges)
                {
                    var start = edge.Start.Position * _tileSizeVector;
                    var end = edge.End.Position * _tileSizeVector;
                    if (edge.IsPortal)
                    {
                        var radius = Vector2.Distance(start, end) / 2;
                        var midPoint = Vector2.Normalize(start - end) * new Vector2(radius) + end;
                        spriteBatch.DrawCircle(midPoint, radius, 20, Color.DarkGreen, 1);
                    }
                    else
                    {
                        spriteBatch.DrawLine(start, end, Color.Green, 2);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }

}